using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication
{
    public class GoogleUserInfoAccessor : ExternalUserInfoAccessor<GoogleUserInfoResponse>
    {

#if !NET6_0_OR_GREATER
        private static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions();
#endif

        public GoogleUserInfoAccessor(
            ILogger<GoogleUserInfoAccessor> logger,
            IExternalUserAuthenticationConfiguration configuration,
            IHttpClientFactory? httpClientFactory,
            string accessToken)
#if NET6_0_OR_GREATER
            : base(logger, configuration, httpClientFactory, GoogleUserInfoSerializerContext.Default.GoogleUserInfoResponse, accessToken)
#else
            : base(logger, configuration, httpClientFactory, JsonSerializerOptions, accessToken)
#endif
        { }

        protected override async Task<IExternalUserInfo> GetAsync(string accessToken, CancellationToken cancellationToken)
        {
            try
            {
                // try as access token
                return await base.GetAsync(accessToken, cancellationToken);
            }
            catch (Exception exn0)
            {
                try
                {
                    // try as auth code
                    using var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
                    {
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "code", accessToken },
                            { "client_id",  Configuration.ClientId },
                            { "client_secret", Configuration.ClientSecret },
                            { "grant_type", "authorization_code" }
                        })
                    };
                    using var client = CreateClient();
                    using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false);
                    var tokenResponse = await JsonSerializer.DeserializeAsync<GoogleTokenResponse>(
#if NET6_0_OR_GREATER
                        await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync(cancellationToken),
                        GoogleUserInfoSerializerContext.Default.GoogleTokenResponse,
#else
                        await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync(),
                        JsonSerializerOptions,
#endif
                        cancellationToken
                    );
                    if (tokenResponse is null)
                    {
                        throw new InvalidOperationException("Unable to parse token response.");
                    }
                    if (tokenResponse.AccessToken is null)
                    {
                        throw new InvalidOperationException("Empty access token in token response.");
                    }
                    return await base.GetAsync(tokenResponse.AccessToken, cancellationToken);
                }
                catch (Exception exn1)
                {
                    throw new AggregateException("Failed to get user info.", exn0, exn1);
                }
            }
        }
    }
}