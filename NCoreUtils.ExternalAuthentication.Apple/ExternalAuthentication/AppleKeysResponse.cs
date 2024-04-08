using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication;

public class AppleKeysResponse
{
    [JsonPropertyName("keys")]
    public List<AppleKeyData>? Keys { get; set; }
}