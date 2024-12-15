using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NCoreUtils.ExternalAuthentication.JwtKeys;

namespace NCoreUtils.ExternalAuthentication;

public class JwtUserInfoAccessor : IExternalUserInfoAccessor
{
    private static JwtSecurityTokenHandler JwtTokenHandler { get; } = new();

    private JwtConfiguration Configuration { get; }

    public JwtKeyCache JwtKeyCache { get; }

    private IHttpClientFactory? HttpClientFactory { get; }

    private string JwtToken { get; }

    public JwtUserInfoAccessor(
        JwtConfiguration configuration,
        JwtKeyCache jwtKeyCache,
        IHttpClientFactory? httpClientFactory,
        string jwtToken)
    {
        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new ArgumentException($"'{nameof(jwtToken)}' cannot be null or whitespace.", nameof(jwtToken));
        }
        HttpClientFactory = httpClientFactory;
        JwtToken = jwtToken;
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        JwtKeyCache = jwtKeyCache;
    }

    public async Task<IExternalUserInfo> GetAsync(CancellationToken cancellationToken)
    {
        var keys = (await JwtKeyCache.GetKeysAsync(HttpClientFactory))
            .Select(static k => new JsonWebKey { Kty = k.Kty, Kid = k.Kid, Use = k.Use, Alg = k.Alg, N = k.N, E = k.E })
            .ToList();
        var jwtToken = JwtTokenHandler.ReadJwtToken(JwtToken);
        var key = keys.FirstOrDefault(k => k.Kid == jwtToken.Header.Kid)
            ?? throw new InvalidOperationException($"No public key with id = {jwtToken.Header.Kid} found.");
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuers = Configuration.ValidIssuers,
            ValidateIssuer = true,
            IssuerSigningKey = key,
            ValidateIssuerSigningKey = true
        };
        if (Configuration.ValidAudiences is IEnumerable<string> validAudiences)
        {
            validationParameters.ValidAudiences = Configuration.ValidAudiences;
            validationParameters.ValidateAudience = true;
        }
        else
        {
            validationParameters.ValidateAudience = false;
        }
        var res = await JwtTokenHandler.ValidateTokenAsync(JwtToken, validationParameters);
        if (!res.IsValid)
        {
            throw new InvalidOperationException($"Failed to verify {Configuration.Provider} id token.", res.Exception);
        }
        return new JwtUserInfo(
            provider: Configuration.Provider,
            id: jwtToken.Subject ?? res.ClaimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? string.Empty,
            email: jwtToken.Claims.FirstOrDefault(e => e.Type == "email")?.Value ?? res.ClaimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? string.Empty
        );
    }
}