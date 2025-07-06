using Gamification.Student.UI.Models.Auth;
using Gamification.Student.UI.Models.Telegram;
using Gamification.Student.UI.Services.Auth;
using Microsoft.JSInterop;

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
                var telegramData = await _jsRuntime.InvokeAsync<WebAppInitData>("getTelegramData");

                await LoginAsync(telegramData);

                return telegramData.user;
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Error fetching Telegram data: {ex.Message}");
                return null;
            }
        }

        private async Task LoginAsync(WebAppInitData telegramData)
        {
            try
            {
                var tokenDto = await _authService.LoginAsync(telegramData);
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
    }
}
