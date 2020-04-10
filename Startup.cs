using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

[assembly: FunctionsStartup(typeof(CseSample.Startup))]
namespace CseSample
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration _configuration;
        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = configurationBuilder.Build();
        }

        // Utilize dependency injection
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) =>
            {
                return CreateIdentityClient();
            });
        }

        private IConfidentialClientApplication CreateIdentityClient()
        {
            try
            {
                string clientId = _configuration.GetValue<string>("ClientId");
                string clientSecret = _configuration.GetValue<string>("ClientSecret");

                return ConfidentialClientApplicationBuilder.Create(clientId)
                                          .WithClientSecret(clientSecret)
                                          .Build();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Please input ClientId and Client Secret in your local.settings.json");
                throw;
            }
        }
    }
}