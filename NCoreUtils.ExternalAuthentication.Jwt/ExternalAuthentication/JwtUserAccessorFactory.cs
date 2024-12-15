using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication;

public class JwtUserAccessorFactory(
    JwtConfiguration configuration,
    ILoggerFactory loggerFactory,
    IHttpClientFactory? httpClientFactory = default
) : IExternalUserInfoAccessorFactory
{
    public JwtKeys.JwtKeyCache KeyCache = new(configuration, loggerFactory.CreateLogger<JwtKeys.JwtKeyCache>());

    public JwtConfiguration Configuration { get; } = configuration;

    public IHttpClientFactory? HttpClientFactory { get; } = httpClientFactory;

    public string ProviderName => Configuration.Provider;

    public IExternalUserInfoAccessor Create(string jwtToken)
        => new JwtUserInfoAccessor(Configuration, KeyCache, HttpClientFactory, jwtToken);
}