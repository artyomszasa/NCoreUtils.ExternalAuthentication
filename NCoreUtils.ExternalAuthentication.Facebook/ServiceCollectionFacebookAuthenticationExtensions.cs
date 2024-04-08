using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.ExternalAuthentication;

namespace NCoreUtils;

public static class ServiceCollectionFacebookAuthenticationExtensions
{
    private const string DefaultInfoEndpoint = "https://graph.facebook.com/v13.0/me?fields=id,email,name,first_name,last_name";

    public static IServiceCollection AddFacebookAuthentication(
        this IServiceCollection services,
        IExternalUserAuthenticationConfiguration configuration)
    {
        services.AddSingleton<IExternalUserInfoAccessorFactory, FacebookUserInfoAccessorFactory>();
        services.AddOptions<ExternalUserAuthenticationConfiguration>(FacebookUserInfoAccessorFactory.ProviderName)
            .Configure(o =>
            {
                o.UserInfoEndpoint = configuration.UserInfoEndpoint?.AbsoluteUri ?? DefaultInfoEndpoint;
                o.ClientId = configuration.ClientId;
                o.ClientSecret = configuration.ClientSecret;
            });
        return services;
    }

    public static IServiceCollection AddFacebookAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = new ExternalUserAuthenticationConfiguration();
        if (configuration is IConfigurationSection section)
        {
            config.UserInfoEndpoint = section[nameof(ExternalUserAuthenticationConfiguration.UserInfoEndpoint)] ?? DefaultInfoEndpoint;
            config.ClientId = section[nameof(ExternalUserAuthenticationConfiguration.ClientId)] ?? string.Empty;
            config.ClientSecret = section[nameof(ExternalUserAuthenticationConfiguration.ClientSecret)] ?? string.Empty;
        }
        return services.AddFacebookAuthentication(config);
    }

    public static IServiceCollection AddFacebookAuthentication(
        this IServiceCollection services,
        string clientId,
        string clientSecret,
        string? userInfoEndpoint = default)
    {
        var config = new ExternalUserAuthenticationConfiguration
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            UserInfoEndpoint = userInfoEndpoint ?? string.Empty
        };
        return services.AddFacebookAuthentication(config);
    }
}