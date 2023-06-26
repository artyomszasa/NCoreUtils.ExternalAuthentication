using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils
{
    public interface IExternalUserAuthentication
    {
        Task<IExternalUserInfo> GetExternalUserInfoAsync(
            string provider,
            string passcode,
            CancellationToken cancellationToken = default);
    }
}