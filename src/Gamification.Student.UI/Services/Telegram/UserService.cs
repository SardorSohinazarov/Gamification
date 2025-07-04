using Common.ServiceAttribute;
using Gamification.Student.UI.Models;
using Microsoft.JSInterop;

namespace Gamification.Student.UI.Services.Telegram
{
    [SingletonService]
    public class UserService : IUserService
    {
        private readonly IJSRuntime _jsRuntime;
        private TelegramUser _cachedUser;

        public UserService(IJSRuntime jsRuntime)
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
