using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication;

public sealed class GoogleTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; }

    [JsonPropertyName("id_token")]
    public string? IdToken { get; }

    [JsonPropertyName("scope")]
    public string? Scope { get; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; }

    public GoogleTokenResponse(string? accessToken, int expiresIn, string? idToken, string? scope, string? tokenType, string? refreshToken)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        IdToken = idToken;
        Scope = scope;
        TokenType = tokenType;
        RefreshToken = refreshToken;
    }
}