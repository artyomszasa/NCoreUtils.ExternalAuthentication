using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCoreUtils.ExternalAuthentication;

namespace NCoreUtils;

public static class ServiceCollectionJwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services, JwtConfiguration configuration)
        => services.AddSingleton<IExternalUserInfoAccessorFactory>(serviceProvider => new JwtUserAccessorFactory(
            configuration: configuration,
            loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>(),
            httpClientFactory: serviceProvider.GetService<IHttpClientFactory>()
        ));

    public static IServiceCollection AddJwtTokenAuthentication(
        this IServiceCollection services,
        string provider,
        string keysEndpoint,
        IReadOnlyList<string> validIssuers,
        IReadOnlyList<string>? validAudiences = default)
        => services.AddJwtTokenAuthentication(new JwtConfiguration(
            provider,
            keysEndpoint,
            validIssuers,
            validAudiences
        ));
}