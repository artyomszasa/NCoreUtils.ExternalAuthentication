namespace NCoreUtils
{
    public interface IExternalUserInfo
    {
        string Provider { get; }

        string Id { get; }

        string Email { get; }

        string FamilyName { get; }

        string GivenName { get; }
    }
}