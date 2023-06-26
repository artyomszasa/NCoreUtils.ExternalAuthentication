using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NCoreUtils.ExternalAuthentication
{
    public class FacebookUserInfoAccessorFactory : ExternalUserInfoAccessorFactory
    {
        public new const string ProviderName = "facebook";

        public FacebookUserInfoAccessorFactory(
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
            => new FacebookUserInfoAccessor(loggerFactory.CreateLogger<FacebookUserInfoAccessor>(), configuration, httpClientFactory, accessToken);
    }
}