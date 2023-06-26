using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.ExternalAuthentication;

namespace NCoreUtils
{
    public static class ServiceCollectionGoogleAuthenticationExtensions
    {
        private const string DefaultInfoEndpoint = "https://www.googleapis.com/userinfo/v2/me";

        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            IExternalUserAuthenticationConfiguration configuration)
        {
            services.AddSingleton<IExternalUserInfoAccessorFactory, GoogleUserInfoAccessorFactory>();
            services.AddOptions<ExternalUserAuthenticationConfiguration>(GoogleUserInfoAccessorFactory.ProviderName)
                .Configure(o =>
                {
                    o.UserInfoEndpoint = configuration.UserInfoEndpoint?.AbsoluteUri ?? DefaultInfoEndpoint;
                    o.ClientId = configuration.ClientId;
                    o.ClientSecret = configuration.ClientSecret;
                });
            return services;
        }

        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var config = new ExternalUserAuthenticationConfiguration();
            if (configuration is IConfigurationSection section)
            {
                config.UserInfoEndpoint = section[nameof(ExternalUserAuthenticationConfiguration.UserInfoEndpoint)] ?? DefaultInfoEndpoint;
                config.ClientId = section[nameof(ExternalUserAuthenticationConfiguration.ClientId)];
                config.ClientSecret = section[nameof(ExternalUserAuthenticationConfiguration.ClientSecret)];
            }
            return services.AddGoogleAuthentication(config);
        }

        public static IServiceCollection AddGoogleAuthentication(
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
            return services.AddGoogleAuthentication(config);
        }
    }
}