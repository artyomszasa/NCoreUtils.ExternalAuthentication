using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication;

public class AppleUserInfoAccessorFactory(
    ILoggerFactory loggerFactory,
    IOptionsMonitor<AppleExternalUserAuthenticationConfiguration> configurationOptions,
    IHttpClientFactory? httpClientFactory)
    : ExternalUserInfoAccessorFactory(ProviderName, loggerFactory, configurationOptions, httpClientFactory)
{
    public new const string ProviderName = "apple";

    protected override IExternalUserInfoAccessor CreateAccessor(
        ILoggerFactory loggerFactory,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
        => new AppleUserInfoAccessor((AppleExternalUserAuthenticationConfiguration)configuration, httpClientFactory, accessToken);
}