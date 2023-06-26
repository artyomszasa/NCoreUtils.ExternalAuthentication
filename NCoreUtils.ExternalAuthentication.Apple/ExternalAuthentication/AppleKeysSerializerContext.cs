using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(AppleKeysResponse))]
public partial class AppleKeysSerializerContext : JsonSerializerContext { }