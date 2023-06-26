using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils.ExternalAuthentication
{
    public interface IExternalUserInfoAccessor
    {
        Task<IExternalUserInfo> GetAsync(CancellationToken cancellationToken);
    }
}