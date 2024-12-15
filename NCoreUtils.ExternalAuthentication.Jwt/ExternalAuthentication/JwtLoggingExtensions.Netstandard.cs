using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication;

internal static partial class JwtLoggingExtensions
{
    public static void LogUsingCachedKeys(this ILogger logger, DateTimeOffset lastFetched, string keys)
        => logger.LogDebug(
            new EventId(UsingCachedKeys, nameof(UsingCachedKeys)),
            "Using cached keys (Last fetched = {LastFetched}, Keys = {Keys}).",
            lastFetched,
            keys
        );
}