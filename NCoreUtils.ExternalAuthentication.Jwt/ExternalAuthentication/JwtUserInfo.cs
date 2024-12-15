namespace NCoreUtils.ExternalAuthentication;

public class JwtUserInfo(string provider, string id, string email) : IExternalUserInfo
{
    public string Provider { get; } = provider;

    public string Id => id;

    public string? Email => email;

    string? IExternalUserInfo.FamilyName => default;

    string? IExternalUserInfo.GivenName => default;
}