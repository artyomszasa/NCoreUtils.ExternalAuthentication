namespace NCoreUtils.ExternalAuthentication;

public class ExternalUserAuthenticationConfiguration : IExternalUserAuthenticationConfiguration
{
    Uri? IExternalUserAuthenticationConfiguration.UserInfoEndpoint
        => string.IsNullOrEmpty(UserInfoEndpoint) ? default : new Uri(UserInfoEndpoint, UriKind.Absolute);

    public string UserInfoEndpoint { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}