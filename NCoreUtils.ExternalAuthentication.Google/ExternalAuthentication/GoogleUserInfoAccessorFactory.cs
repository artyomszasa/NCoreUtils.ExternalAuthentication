using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication
{
    public class GoogleUserInfoAccessorFactory : ExternalUserInfoAccessorFactory
    {
        public new const string ProviderName = "google";

        public GoogleUserInfoAccessorFactory(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<ExternalUserAuthenticationConfiguration> configurationOptions,
            IHttpClientFactory? httpClientFactory)
            : base(ProviderName, loggerFactory, configurationOptions, httpClientFactory)
        { }

        protected override IExternalUserInfoAccessor CreateAccessor(
            ILoggerFactory loggerFactory,
            IExternalUserAuthenticationConfiguration configuration,
            IHttpClientFactory? httpClientFactory,
            string accessToken)
            => new GoogleUserInfoAccessor(loggerFactory.CreateLogger<GoogleUserInfoAccessor>(), configuration, httpClientFactory, accessToken);
    }
}