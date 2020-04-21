using System.Threading.Tasks;

namespace CseSample.Services
{
    public interface ICallService
    {
        Task<bool> startGroupCallWithSpecificMembers(string[] userIds, string tenantId, string accessToken);
    }
}