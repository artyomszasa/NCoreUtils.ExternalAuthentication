using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication;

public class ExternalUserAuthentication(
    IEnumerable<IExternalUserInfoAccessorFactory> accessorFactories,
    ILogger<ExternalUserAuthentication> logger)
    : IExternalUserAuthentication
{
    protected IEnumerable<IExternalUserInfoAccessorFactory> AccessorFactories { get; } = accessorFactories ?? throw new ArgumentNullException(nameof(accessorFactories));

    protected ILogger Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task<IExternalUserInfo> GetExternalUserInfoAsync(string provider, string passcode, CancellationToken cancellationToken = default)
    {
        foreach (var accessorFactory in AccessorFactories)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Equals(accessorFactory.ProviderName, provider))
            {
                return accessorFactory.Create(passcode).GetAsync(cancellationToken);
            }
        }
        throw new NotSupportedException($"No provider found for '${provider}'.");
    }
}