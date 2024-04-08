namespace NCoreUtils.ExternalAuthentication;

public class AppleUserInfo(string id, string email) : IExternalUserInfo
{
    public string Provider => "apple";

    public string Id { get; } = id;

    public string Email { get; } = email;

    public string? FamilyName => default;

    public string? GivenName => default;
}