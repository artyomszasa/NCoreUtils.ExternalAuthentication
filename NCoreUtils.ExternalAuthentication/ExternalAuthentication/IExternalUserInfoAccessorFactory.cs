namespace NCoreUtils.ExternalAuthentication;

public interface IExternalUserInfoAccessorFactory
{
    string ProviderName { get; }

    IExternalUserInfoAccessor Create(string accessToken);
}