namespace NCoreUtils.ExternalAuthentication;

public interface IExternalUserInfoAccessor
{
    Task<IExternalUserInfo> GetAsync(CancellationToken cancellationToken);
}