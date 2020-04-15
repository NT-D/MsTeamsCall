using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace CseSample
{
    public class TokenService : ITokenService
    {
        private readonly IConfidentialClientApplication _confidentialClient;
        
        public TokenService(IConfidentialClientApplication confidentialClient)
        {
            _confidentialClient = confidentialClient;
        }

        public async Task<string> FetchAccessTokenByTenantId(string tenantId)
        {
            // TODO: Use tenantId for fetching tenant specific token

            // AcquireTokenForClient().ExecuteAsync() hard to mock
            // Because AcquireTokenForClient() returns sealed class, so we can't mock it and it's ExecuteAsync()
            var result = await _confidentialClient.AcquireTokenForClient(new List<string>() { "https://graph.microsoft.com/.default" }).ExecuteAsync();
            return result.AccessToken;
        }
    }
}