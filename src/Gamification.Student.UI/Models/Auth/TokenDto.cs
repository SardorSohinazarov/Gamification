using System.Text.Json.Serialization;

namespace Gamification.Student.UI.Models.Auth
{
    public class TokenDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("accessTokenExpireDate")]
        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
