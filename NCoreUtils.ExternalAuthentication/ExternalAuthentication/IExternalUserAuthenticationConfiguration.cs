namespace NCoreUtils.ExternalAuthentication;

public interface IExternalUserAuthenticationConfiguration
{
    Uri? UserInfoEndpoint { get; }

    string ClientId { get; }

    string ClientSecret { get; }
}