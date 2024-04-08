namespace NCoreUtils.ExternalAuthentication;

public class AppleExternalUserAuthenticationConfiguration
    : ExternalUserAuthenticationConfiguration
    , IAppleExternalUserAuthenticationConfiguration
{
    public List<string> ValidAudiences { get; set; } = [];

    IReadOnlyList<string> IAppleExternalUserAuthenticationConfiguration.ValidAudiences => ValidAudiences;
}