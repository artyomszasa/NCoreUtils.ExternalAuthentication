using System.Collections.Generic;

namespace NCoreUtils.ExternalAuthentication;

public class AppleExternalUserAuthenticationConfiguration
    : ExternalUserAuthenticationConfiguration
    , IAppleExternalUserAuthenticationConfiguration
{
    public List<string> ValidAudiences { get; set; } = new();

    IReadOnlyList<string> IAppleExternalUserAuthenticationConfiguration.ValidAudiences => ValidAudiences;
}