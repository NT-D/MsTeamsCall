using System.Threading.Tasks;

namespace CseSample
{
    public interface ITokenService
    {
        Task<string> FetchAccessTokenByTenantId(string tenantId);
    }
}