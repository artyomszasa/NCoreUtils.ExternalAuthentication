using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(FacebookUserInfoResponse))]
public partial class FacebookUserInfoSerializerContext : JsonSerializerContext { }