using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(GoogleUserInfoResponse))]
[JsonSerializable(typeof(GoogleTokenResponse))]
public partial class GoogleUserInfoSerializerContext : JsonSerializerContext { }