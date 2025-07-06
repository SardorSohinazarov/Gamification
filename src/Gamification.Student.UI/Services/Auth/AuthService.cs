using Gamification.Student.UI.Models.Auth;
using Gamification.Student.UI.Models.Telegram;
using System.Net.Http.Json;

namespace Gamification.Student.UI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenDto> LoginAsync(WebAppUser telegramAuthData, string initData)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/telegram", new{ user = telegramAuthData, initData = initData });
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TokenDto>();
                }
                else
                {
                    throw new Exception($"Login failed: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during login: {ex.Message}");
                throw;
            }
        }
    }
}
