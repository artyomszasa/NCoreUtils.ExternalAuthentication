using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication.JwtKeys;

public class JwtKeysResponse
{
    [JsonPropertyName("keys")]
    public List<JwtKeyData>? Keys { get; set; }
}