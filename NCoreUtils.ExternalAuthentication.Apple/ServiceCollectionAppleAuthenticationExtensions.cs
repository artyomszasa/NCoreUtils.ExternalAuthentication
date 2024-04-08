using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.ExternalAuthentication;

namespace NCoreUtils;

public static class ServiceCollectionAppleAuthenticationExtensions
{
    public static IServiceCollection AddAppleAuthentication(
        this IServiceCollection services,
        IAppleExternalUserAuthenticationConfiguration configuration)
    {
        services.AddSingleton<IExternalUserInfoAccessorFactory, AppleUserInfoAccessorFactory>();
        services.AddOptions<AppleExternalUserAuthenticationConfiguration>(AppleUserInfoAccessorFactory.ProviderName)
            .Configure(o =>
            {
                o.UserInfoEndpoint = configuration.UserInfoEndpoint?.AbsoluteUri ?? string.Empty;
                o.ClientId = configuration.ClientId;
                o.ClientSecret = configuration.ClientSecret;
                o.ValidAudiences = [.. configuration.ValidAudiences];
            });
        return services;
    }

    public static IServiceCollection AddAppleAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = new AppleExternalUserAuthenticationConfiguration();
        if (configuration is IConfigurationSection section)
        {
            config.UserInfoEndpoint = section[nameof(ExternalUserAuthenticationConfiguration.UserInfoEndpoint)] ?? string.Empty;
            config.ClientId = section[nameof(ExternalUserAuthenticationConfiguration.ClientId)] ?? string.Empty;
            config.ClientSecret = section[nameof(ExternalUserAuthenticationConfiguration.ClientSecret)] ?? string.Empty;
            config.ValidAudiences = section.GetSection("ValidAudiences")
                .GetChildren()
                .Select(section => section.Value)
                .Where(value => !string.IsNullOrEmpty(value))
                .ToList()!;
        }
        return services.AddAppleAuthentication(config);
    }

    public static IServiceCollection AddAppleAuthentication(
        this IServiceCollection services,
        string clientId,
        string clientSecret,
        IEnumerable<string> validAudiences)
    {
        var config = new AppleExternalUserAuthenticationConfiguration
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            ValidAudiences = new(validAudiences)
        };
        return services.AddAppleAuthentication(config);
    }
}