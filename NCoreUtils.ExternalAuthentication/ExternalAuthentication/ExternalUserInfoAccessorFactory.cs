using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication;

public abstract class ExternalUserInfoAccessorFactory(
    string providerName,
    ILoggerFactory loggerFactory,
    IOptionsMonitor<ExternalUserAuthenticationConfiguration> configurationOptions,
    IHttpClientFactory? httpClientFactory)
    : IExternalUserInfoAccessorFactory
{
    protected ILoggerFactory LoggerFactory { get; } = loggerFactory;

    protected IExternalUserAuthenticationConfiguration Configuration { get; } = configurationOptions.Get(providerName);

    protected IHttpClientFactory? HttpClientFactory { get; } = httpClientFactory;

    public string ProviderName { get; } = providerName;

    protected abstract IExternalUserInfoAccessor CreateAccessor(
        ILoggerFactory loggerFactory,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken);

    public IExternalUserInfoAccessor Create(string accessToken)
        => CreateAccessor(LoggerFactory, Configuration, HttpClientFactory, accessToken);
}