using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication
{
    public class ExternalUserAuthentication : IExternalUserAuthentication
    {
        protected IEnumerable<IExternalUserInfoAccessorFactory> AccessorFactories { get; }

        protected ILogger Logger { get; }

        public ExternalUserAuthentication(IEnumerable<IExternalUserInfoAccessorFactory> accessorFactories, ILogger<ExternalUserAuthentication> logger)
        {
            AccessorFactories = accessorFactories ?? throw new ArgumentNullException(nameof(accessorFactories));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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
}