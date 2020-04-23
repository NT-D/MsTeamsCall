using System.Threading.Tasks;

namespace CseSample.Services
{
    public interface ICallService
    {
        Task<bool> StartGroupCallWithSpecificMembers(string[] userIds, string tenantId, string accessToken);
    }
}