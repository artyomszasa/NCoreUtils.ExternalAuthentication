namespace NCoreUtils.ExternalAuthentication;

public class AppleUserInfo : IExternalUserInfo
{
    public string Provider => "apple";

    public string Id { get; }

    public string Email { get; }

    public string FamilyName => string.Empty;

    public string GivenName => string.Empty;

    public AppleUserInfo(string id, string email)
    {
        Id = id;
        Email = email;
    }
}