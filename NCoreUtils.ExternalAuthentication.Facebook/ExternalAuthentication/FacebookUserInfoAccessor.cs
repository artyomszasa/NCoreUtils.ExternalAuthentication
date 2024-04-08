using Microsoft.Extensions.Logging;

#if !NET6_0_OR_GREATER
using System.Text.Json;
#endif

namespace NCoreUtils.ExternalAuthentication;

public class FacebookUserInfoAccessor : ExternalUserInfoAccessor<FacebookUserInfoResponse>
{
#if !NET6_0_OR_GREATER
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions();
#endif

    public FacebookUserInfoAccessor(
        ILogger<FacebookUserInfoAccessor> logger,
        IExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
#if NET6_0_OR_GREATER
        : base(logger, configuration, httpClientFactory, FacebookUserInfoSerializerContext.Default.FacebookUserInfoResponse, accessToken)
#else
        : base(logger, configuration, httpClientFactory, JsonSerializerOptions, accessToken)
#endif
    { }
}