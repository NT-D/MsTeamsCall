using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CseSample.Services;
using CseSample.Models;
using System.IO;
using Newtonsoft.Json;

namespace CseSample
{
    public class CallFunction
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersService _usersService;
        private readonly ICallService _callService;
        private readonly IMeetingService _meetingService;
        public CallFunction(ITokenService tokenService, IUsersService usersService, ICallService callService, IMeetingService meetingService)
        {
            // Utilize dependency injection
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            _tokenService = tokenService;
            _usersService = usersService;
            _callService = callService;
            _meetingService = meetingService;
        }

        [FunctionName(nameof(Calls))]
        public async Task<IActionResult> Calls(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calls")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var callRequest = JsonConvert.DeserializeObject<GroupCall>(requestBody);
            if (!callRequest.IsValid()) return new BadRequestResult();

            try
            {
                string accessToken = await _tokenService.FetchAccessTokenByTenantId(callRequest.TenantId);
                string[] userIds = await _usersService.GetUserIdsFromEmailAsync(callRequest.ParticipantEmails, accessToken);
                await _callService.StartGroupCallWithSpecificMembers(userIds, callRequest.TenantId, accessToken).ConfigureAwait(false);
                return new OkObjectResult("test");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw;
            }
        }

        [FunctionName(nameof(CallWithMeetingAttendees))]
        public async Task<IActionResult> CallWithMeetingAttendees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calls/{meetingId}")] HttpRequest req, string meetingId,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var meetingCallRequest = JsonConvert.DeserializeObject<MeetingCall>(requestBody);
            if (!meetingCallRequest.IsValid() || meetingId != meetingCallRequest.MeetingId) return new BadRequestObjectResult("Please request with valid tenantId, eventId and userId");

            try
            {
                string accessToken = await _tokenService.FetchAccessTokenByTenantId(meetingCallRequest.TenantId);
                string userId = (await _usersService.GetUserIdsFromEmailAsync(new string[] { meetingCallRequest.UserEmail }, accessToken))[0];
                Meeting meetingInfo = await _meetingService.GetOnlineMeetingInfo(meetingId, userId, accessToken);

                if (meetingInfo.IsOnlineMeetingSet)
                {
                    await _callService.JoinExistingOnlineMeeting(userId, meetingInfo, accessToken);
                }
                else
                {
                    string[] attendeesIds = await _usersService.GetUserIdsFromEmailAsync(meetingInfo.AttendeeEmails, accessToken);
                    await _callService.StartGroupCallWithSpecificMembers(attendeesIds, meetingCallRequest.TenantId, accessToken).ConfigureAwait(false);
                }

                return new OkObjectResult("test");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw;
            }
        }
    }
}
