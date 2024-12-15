using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication;

internal static partial class JwtLoggingExtensions
{
    [LoggerMessage(
        EventId = UsingCachedKeys,
        EventName = nameof(UsingCachedKeys),
        Level = LogLevel.Debug,
        Message = "Using cached keys (Last fetched = {LastFetched}, Keys = {Keys}).",
        SkipEnabledCheck = true
    )]
    public static partial void LogUsingCachedKeys(this ILogger logger, DateTimeOffset lastFetched, string keys);
}