using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication;

public class FacebookUserInfoAccessorFactory(
    ILoggerFactory loggerFactory,
    IOptionsMonitor<ExternalUserAuthenticationConfiguration> configurationOptions,
    IHttpClientFactory? httpClientFactory)
    : ExternalUserInfoAccessorFactory(ProviderName, loggerFactory, configurationOptions, httpClientFactory)
{
    public new const string ProviderName = "facebook";

    protected override IExternalUserInfoAccessor CreateAccessor(
        ILoggerFactory loggerFactory,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
        => new FacebookUserInfoAccessor(loggerFactory.CreateLogger<FacebookUserInfoAccessor>(), configuration, httpClientFactory, accessToken);
}