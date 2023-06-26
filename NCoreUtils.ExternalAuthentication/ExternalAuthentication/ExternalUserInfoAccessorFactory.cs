using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication
{
    public abstract class ExternalUserInfoAccessorFactory : IExternalUserInfoAccessorFactory
    {
        protected ILoggerFactory LoggerFactory { get; }

        protected IExternalUserAuthenticationConfiguration Configuration { get; }

        protected IHttpClientFactory? HttpClientFactory { get; }

        public string ProviderName { get; }

        public ExternalUserInfoAccessorFactory(
            string providerName,
            ILoggerFactory loggerFactory,
            IOptionsMonitor<ExternalUserAuthenticationConfiguration> configurationOptions,
            IHttpClientFactory? httpClientFactory)
        {
            ProviderName = providerName;
            LoggerFactory = loggerFactory;
            Configuration = configurationOptions.Get(providerName);
            HttpClientFactory = httpClientFactory;
        }

        protected abstract IExternalUserInfoAccessor CreateAccessor(
            ILoggerFactory loggerFactory,
            IExternalUserAuthenticationConfiguration configuration,
            IHttpClientFactory? httpClientFactory,
            string accessToken);

        public IExternalUserInfoAccessor Create(string accessToken)
            => CreateAccessor(LoggerFactory, Configuration, HttpClientFactory, accessToken);
    }
}