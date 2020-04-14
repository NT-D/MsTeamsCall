using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace CseSample
{
    public class CallFunction
    {
        private readonly ITokenService _tokenService;
        public CallFunction(ITokenService tokenService)
        {
            // Utilize dependency injection
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            _tokenService = tokenService;
        }

        [FunctionName("CallFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // TODO: Will specify tenant id
                string accessToken = await _tokenService.FetchAccessTokenByTenantId("willUpdateWithTenantId");
                return new OkObjectResult(accessToken);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                throw;
            }
        }
    }
}
