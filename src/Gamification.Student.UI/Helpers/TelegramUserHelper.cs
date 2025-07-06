using Gamification.Student.UI.Models.Auth;
using Gamification.Student.UI.Models.Telegram;
using Gamification.Student.UI.Services.Auth;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Gamification.Student.UI.Helpers
{
    public class TelegramUserHelper
    {
        private WebAppUser _cachedUser;
        private readonly IJSRuntime _jsRuntime;
        private readonly IAuthService _authService;

        public TelegramUserHelper(IJSRuntime jsRuntime, IAuthService authService)
        {
            _jsRuntime = jsRuntime;
            _authService = authService;
        }

        public async Task<WebAppUser> GetUserAsync()
        {
            if (_cachedUser != null)
            {
                return _cachedUser;
            }

            try
            {
                var webAppInitDataString = await _jsRuntime.InvokeAsync<string>("getTelegramData");

                Console.Error.WriteLine($"WebAppInitData: {webAppInitDataString}");
                if (string.IsNullOrEmpty(webAppInitDataString))
                {
                    Console.Error.WriteLine("WebAppInitData is null or empty.");
                    return null;
                }

                var webAppInitData = JsonSerializer.Deserialize<WebAppInitData>(webAppInitDataString);
                var user = webAppInitData.user;

                await LoginAsync(user, webAppInitDataString);

                return user;
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Error fetching Telegram data: {ex.Message}");
                return null;
            }
        }

        private async Task LoginAsync(WebAppUser telegramData, string initData)
        {
            try
            {
                var tokenDto = await _authService.LoginAsync(telegramData, initData);
                if (tokenDto != null && !string.IsNullOrEmpty(tokenDto.AccessToken))
                {
                    await SetTokenAsync(tokenDto);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<TokenDto> GetTokenAsync()
        {
            var accessToken = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "accessToken");
            var refreshToken = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "refreshToken");
            var refreshTokenExpireDateStr = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "refreshTokenExpireDate");
            if (!DateTime.TryParse(refreshTokenExpireDateStr, out var refreshTokenExpireDate))
            {
                return null;
            }
            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpireDate = refreshTokenExpireDate
            };
        }

        public async Task SetTokenAsync(TokenDto tokenDto)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "accessToken", tokenDto.AccessToken);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "refreshToken", tokenDto.RefreshToken);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "refreshTokenExpireDate", tokenDto.RefreshTokenExpireDate.ToString("o"));
        }

        private WebAppInitData ParseInitData(string initDataString)
        {
            var query = System.Web.HttpUtility.ParseQueryString(initDataString);

            var initData = new WebAppInitData
            {
                auth_date = query["auth_date"],
                hash = query["hash"],
                query_id = query["query_id"],
                chat_type = query["chat_type"],
                chat_instance = query["chat_instance"],
                start_param = query["start_param"],
                signature = query["signature"],
                can_send_after = int.TryParse(query["can_send_after"], out var csa) ? csa : null
            };

            // Parse user
            if (query["user"] is string userJson)
            {
                initData.user = System.Text.Json.JsonSerializer.Deserialize<WebAppUser>(userJson);
            }

            // Parse chat (optional)
            if (query["chat"] is string chatJson)
            {
                initData.chat = System.Text.Json.JsonSerializer.Deserialize<WebAppChat>(chatJson);
            }

            // Parse receiver (optional)
            if (query["receiver"] is string receiverJson)
            {
                initData.receiver = System.Text.Json.JsonSerializer.Deserialize<WebAppUser>(receiverJson);
            }

            return initData;
        }

    }
}
