using System.Text.Json.Serialization;

namespace NCoreUtils.ExternalAuthentication
{
    public class FacebookUserInfoResponse : IExternalUserInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("first_name")]
        public string GivenName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string FamilyName { get; set; } = string.Empty;

        [JsonIgnore]
        public string Provider => "facebook";
    }
}