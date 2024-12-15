namespace NCoreUtils.ExternalAuthentication;

public class JwtConfiguration(
    string provider,
    string keysEndpoint,
    IReadOnlyList<string> validIssuers,
    IReadOnlyList<string>? validAudiences)
{
    public string Provider { get; } = provider;

    public string KeysEndpoint { get; } = keysEndpoint;

    public IReadOnlyList<string> ValidIssuers { get; } = validIssuers ?? [];

    public IReadOnlyList<string>? ValidAudiences { get; } = validAudiences;
}