using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace NCoreUtils.ExternalAuthentication.JwtKeys;

public class JwtKeyCache(JwtConfiguration configuration, ILogger<JwtKeyCache> logger)
{
    private static object Sync { get; } = new object();

    private ILogger Logger { get; } = logger;

    private DateTimeOffset LastKeyFetchTimestamp { get; set; }

    private IReadOnlyList<JwtKeyData>? Keys { get; set; }

    private Task<IReadOnlyList<JwtKeyData>>? PendingKeys { get; set; }

    public JwtConfiguration Configuration { get; } = configuration;

    private Task<JwtKeysResponse?> DoFetchKeysAsync(HttpClient client)
        => client.GetFromJsonAsync(Configuration.KeysEndpoint, JwtKeysSerializerContext.Default.JwtKeysResponse);

    private async Task<IReadOnlyList<JwtKeyData>> FetchKeysAsync(IHttpClientFactory? httpClientFactory)
    {
        await Task.Yield();
        try
        {
            using var client = httpClientFactory switch
            {
                null => new HttpClient(),
                var factory => factory.CreateClient(nameof(ExternalUserAuthentication))
            };
            Keys = (await DoFetchKeysAsync(client))?.Keys ?? (IReadOnlyList<JwtKeyData>)Array.Empty<JwtKeyData>();
            return Keys;
        }
        finally
        {
            PendingKeys = default;
        }
    }

    public ValueTask<IReadOnlyList<JwtKeyData>> GetKeysAsync(IHttpClientFactory? httpClientFactory)
    {
        var now = DateTimeOffset.Now;
        if (Keys is IReadOnlyList<JwtKeyData> cachedKeys && LastKeyFetchTimestamp > now.AddDays(-1))
        {
            // use cached
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                var keys = string.Join(", ", cachedKeys.Select(key => key.Kid));
                Logger.LogUsingCachedKeys(LastKeyFetchTimestamp, keys);
            }
            return new(cachedKeys);
        }
        lock (Sync)
        {
            if (Keys is IReadOnlyList<JwtKeyData> keys && LastKeyFetchTimestamp > now.AddDays(-1))
            {
                // use cached (updated during lock acquiring)
                if (Logger.IsEnabled(LogLevel.Debug))
                {
                    var keyIds = string.Join(", ", keys.Select(key => key.Kid));
                    Logger.LogUsingCachedKeys(LastKeyFetchTimestamp, keyIds);
                }
                return new(keys);
            }
            LastKeyFetchTimestamp = now;
            // use either already started task or initialize new one.
            return new(PendingKeys ??= FetchKeysAsync(httpClientFactory));
        }
    }
}