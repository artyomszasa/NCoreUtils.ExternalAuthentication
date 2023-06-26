using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication;

public class AppleUserInfoAccessorFactory : ExternalUserInfoAccessorFactory
{
    public new const string ProviderName = "apple";

    public AppleUserInfoAccessorFactory(
        ILoggerFactory loggerFactory,
        IOptionsMonitor<AppleExternalUserAuthenticationConfiguration> configurationOptions,
        IHttpClientFactory? httpClientFactory)
        : base(ProviderName, loggerFactory, configurationOptions, httpClientFactory)
    { }

    protected override IExternalUserInfoAccessor CreateAccessor(
        ILoggerFactory loggerFactory,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
        => new AppleUserInfoAccessor((AppleExternalUserAuthenticationConfiguration)configuration, httpClientFactory, accessToken);
}