using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
#if NET6_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NCoreUtils.AspNetCore;

namespace NCoreUtils.ExternalAuthentication
{
    public class ExternalUserInfoAccessor<TResponse> : IExternalUserInfoAccessor
        where TResponse : IExternalUserInfo
    {
#if NET6_0_OR_GREATER
        private static Task<string> ReadContentAsStringAsync(HttpResponseMessage response, CancellationToken cancellationToken)
            => response.Content.ReadAsStringAsync(cancellationToken);

        private static Task<Stream> ReadContentAsStreamAsync(HttpResponseMessage response, CancellationToken cancellationToken)
            => response.Content.ReadAsStreamAsync(cancellationToken);

        private TResponse Deserialize(string body)
            => JsonSerializer.Deserialize(body, _typeInfo) ?? throw new InvalidOperationException("Unable to deserialize response.");

        private ValueTask<TResponse?> DeserializeAsync(Stream body, CancellationToken cancellationToken)
            => JsonSerializer.DeserializeAsync(body, _typeInfo, cancellationToken);
#else
        private static Task<string> ReadContentAsStringAsync(HttpResponseMessage response, CancellationToken cancellationToken)
            => response.Content.ReadAsStringAsync();

        private static Task<Stream> ReadContentAsStreamAsync(HttpResponseMessage response, CancellationToken cancellationToken)
            => response.Content.ReadAsStreamAsync();

        private TResponse Deserialize(string body)
            => JsonSerializer.Deserialize<TResponse>(body, _jsonOptions) ?? throw new InvalidOperationException("Unable to deserialize response.");

        private ValueTask<TResponse?> DeserializeAsync(Stream body, CancellationToken cancellationToken)
            => JsonSerializer.DeserializeAsync<TResponse>(body, _jsonOptions, cancellationToken)!;
#endif

        private readonly ILogger _logger;

        private readonly IExternalUserAuthenticationConfiguration _configuration;

        private readonly IHttpClientFactory? _httpClientFactory;

#if NET6_0_OR_GREATER
        private readonly JsonTypeInfo<TResponse> _typeInfo;
#else
        private readonly JsonSerializerOptions _jsonOptions;
#endif

        private readonly string _accessToken;

        protected IExternalUserAuthenticationConfiguration Configuration => _configuration;

        public ExternalUserInfoAccessor(
            ILogger<ExternalUserInfoAccessor<TResponse>> logger,
            IExternalUserAuthenticationConfiguration configuration,
            IHttpClientFactory? httpClientFactory,
#if NET6_0_OR_GREATER
            JsonTypeInfo<TResponse> typeInfo,
#else
            JsonSerializerOptions jsonOptions,
#endif
            string accessToken)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientFactory = httpClientFactory;
#if NET6_0_OR_GREATER
            _typeInfo = typeInfo;
#else
            _jsonOptions = jsonOptions;
#endif
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        protected HttpClient CreateClient()
            => _httpClientFactory?.CreateClient(nameof(ExternalUserAuthentication)) ?? new HttpClient();

        protected virtual async Task<IExternalUserInfo> GetAsync(string accessToken, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, _configuration.UserInfoEndpoint);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                using var client = CreateClient();
                using var responseMessage = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    var body = await ReadContentAsStringAsync(responseMessage, cancellationToken).ConfigureAwait(false);
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Response body for {Endpoint} was: {Response}.", _configuration.UserInfoEndpoint, body);
                        throw new UnauthorizedException("Remote authentication has failed.");
                    }
                    _logger.LogDebug("Response body for {Endpoint} was: {Response}.", _configuration.UserInfoEndpoint, body);
                    return Deserialize(body);
                }
                else
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        var body = await ReadContentAsStringAsync(responseMessage, cancellationToken).ConfigureAwait(false);
                        _logger.LogWarning("Response body for {Endpoint} was: {Response}.", _configuration.UserInfoEndpoint, body);
                        throw new UnauthorizedException("Remote authentication has failed.");
                    }
                    using var stream = await ReadContentAsStreamAsync(responseMessage, cancellationToken).ConfigureAwait(false);
                    return await DeserializeAsync(stream, cancellationToken)
                        .ConfigureAwait(false)
                        ?? throw new InvalidOperationException("Unable to deserialize response.");
                }
            }
            catch (Exception exn)
            {
                throw new ExternalUserAuthenticationException(
                    $"Failed to get user info from {_configuration.UserInfoEndpoint}",
                    exn
                );
            }
        }

        public Task<IExternalUserInfo> GetAsync(CancellationToken cancellationToken)
            => GetAsync(_accessToken, cancellationToken);
    }
}