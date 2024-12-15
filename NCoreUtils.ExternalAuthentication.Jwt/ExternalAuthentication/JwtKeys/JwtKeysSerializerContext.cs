using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication.JwtKeys;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(JwtKeysResponse))]
public partial class JwtKeysSerializerContext : JsonSerializerContext { }