using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication;

public class GoogleUserInfoAccessorFactory(
    ILoggerFactory loggerFactory,
    IOptionsMonitor<ExternalUserAuthenticationConfiguration> configurationOptions,
    IHttpClientFactory? httpClientFactory)
    : ExternalUserInfoAccessorFactory(ProviderName, loggerFactory, configurationOptions, httpClientFactory)
{
    public new const string ProviderName = "google";

    protected override IExternalUserInfoAccessor CreateAccessor(
        ILoggerFactory loggerFactory,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
        => new GoogleUserInfoAccessor(loggerFactory.CreateLogger<GoogleUserInfoAccessor>(), configuration, httpClientFactory, accessToken);
}