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
        private readonly IConfidentialClientApplication _confidentialClient;
        public CallFunction(IConfidentialClientApplication confidentialClient)
        {
            // Utilize dependency injection
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            _confidentialClient = confidentialClient;
        }

        [FunctionName("CallFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // We'll utilize token in the future PR
            var result = await _confidentialClient.AcquireTokenForClient(new List<string>() { "https://graph.microsoft.com/.default" }).ExecuteAsync();

            string responseMessage = string.IsNullOrEmpty(result.AccessToken)
                ? "Failed"
                : $"{result.AccessToken}";

            return new OkObjectResult(responseMessage);
        }
    }
}
