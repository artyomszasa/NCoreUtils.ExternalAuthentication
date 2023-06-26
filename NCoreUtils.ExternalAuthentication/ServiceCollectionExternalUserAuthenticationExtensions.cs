using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.ExternalAuthentication;

namespace NCoreUtils
{
    public static class ServiceCollectionExternalUserAuthenticationExtensions
    {
        public static IServiceCollection AddExternalUserAuthentication(this IServiceCollection services)
        {
            return services.AddSingleton<IExternalUserAuthentication, ExternalUserAuthentication>();
        }
    }
}