namespace NCoreUtils.ExternalAuthentication;

public interface IAppleExternalUserAuthenticationConfiguration : IExternalUserAuthenticationConfiguration
{
    IReadOnlyList<string> ValidAudiences { get; }
}