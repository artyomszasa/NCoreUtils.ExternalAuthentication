using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Microsoft.IdentityModel.Tokens;

namespace NCoreUtils.ExternalAuthentication;

public class AppleUserInfoAccessor : IExternalUserInfoAccessor
{
    private static object Sync { get; } = new object();

    private static JwtSecurityTokenHandler JwtTokenHandler { get; } = new();

    private static DateTimeOffset LastKeyFetchTimestamp { get; set; }

    private static IReadOnlyList<AppleKeyData>? Keys { get; set; }

    private static Task<IReadOnlyList<AppleKeyData>>? PendingKeys { get; set; }

    private static Task<AppleKeysResponse?> GetKeysAsync(HttpClient client)
        => client.GetFromJsonAsync("https://appleid.apple.com/auth/keys", AppleKeysSerializerContext.Default.AppleKeysResponse);

    private static async Task<IReadOnlyList<AppleKeyData>> FetchAppleKeysAsync(IHttpClientFactory? httpClientFactory)
    {
        await Task.Yield();
        try
        {
            using var client = httpClientFactory switch
            {
                null => new HttpClient(),
                var factory => factory.CreateClient(nameof(ExternalUserAuthentication))
            };
            Keys = (await GetKeysAsync(client))?.Keys ?? (IReadOnlyList<AppleKeyData>)Array.Empty<AppleKeyData>();
            return Keys;
        }
        finally
        {
            PendingKeys = default;
        }
    }

    private static ValueTask<IReadOnlyList<AppleKeyData>> GetAppleKeysAsync(IHttpClientFactory? httpClientFactory)
    {
        var now = DateTimeOffset.Now;
        if (Keys is IReadOnlyList<AppleKeyData> cachedKeys && LastKeyFetchTimestamp < now.AddDays(1))
        {
            // use cached
            return new(cachedKeys);
        }
        lock (Sync)
        {
            if (Keys is IReadOnlyList<AppleKeyData> keys && LastKeyFetchTimestamp < now.AddDays(1))
            {
                // use cached (updated during lock acquiring)
                return new(keys);
            }
            LastKeyFetchTimestamp = now;
            // use either already started task or initialize new one.
            return new(PendingKeys ??= FetchAppleKeysAsync(httpClientFactory));
        }
    }

    private AppleExternalUserAuthenticationConfiguration Configuration { get; }

    private IHttpClientFactory? HttpClientFactory { get; }

    private string IdToken { get; }

    public AppleUserInfoAccessor(
        AppleExternalUserAuthenticationConfiguration configuration,
        IHttpClientFactory? httpClientFactory,
        string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException($"'{nameof(accessToken)}' cannot be null or whitespace.", nameof(accessToken));
        }
        HttpClientFactory = httpClientFactory;
        // NOTE: apple uses id tokens
        IdToken = accessToken;
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<IExternalUserInfo> GetAsync(CancellationToken cancellationToken)
    {
        var keys = (await GetAppleKeysAsync(HttpClientFactory))
            .Select(static k => new JsonWebKey { Kty = k.Kty, Kid = k.Kid, Use = k.Use, Alg = k.Alg, N = k.N, E = k.E })
            .ToList();
        var jwtToken = JwtTokenHandler.ReadJwtToken(IdToken);
        var key = keys.FirstOrDefault(k => k.Kid == jwtToken.Header.Kid);
        if (key is null)
        {
            throw new InvalidOperationException($"No public key with id = {jwtToken.Header.Kid} found.");
        }
        var res = await JwtTokenHandler.ValidateTokenAsync(IdToken, new TokenValidationParameters
        {
            ValidIssuer = "https://appleid.apple.com",
            IssuerSigningKey = key,
            ValidAudiences = Configuration.ValidAudiences
        });
        if (!res.IsValid)
        {
            throw new InvalidOperationException("Failed to verify apple id token.", res.Exception);
        }
        return new AppleUserInfo(
            id: jwtToken.Subject ?? res.ClaimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? string.Empty,
            email: jwtToken.Claims.FirstOrDefault(e => e.Type == "email")?.Value ?? res.ClaimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? string.Empty
        );
    }
}