using Gamification.Student.UI.Models.Telegram;
using Microsoft.JSInterop;

namespace Gamification.Student.UI.Services.Helpers
{
    public class TelegramUserHelper
    {
        private readonly IJSRuntime _jsRuntime;
        private TelegramUser _cachedUser;

        public TelegramUserHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<TelegramUser> GetUserAsync()
        {
            if (_cachedUser != null)
            {
                return _cachedUser;
            }

            try
            {
                _cachedUser = await _jsRuntime.InvokeAsync<TelegramUser>("getTelegramUser");

                return _cachedUser;
            }
            catch (JSException ex)
            {
                Console.Error.WriteLine($"Error fetching Telegram user: {ex.Message}");
                return null;
            }
        }
    }
}
