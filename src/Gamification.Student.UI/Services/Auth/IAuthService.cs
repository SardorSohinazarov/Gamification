using Gamification.Student.UI.Models.Auth;
using Gamification.Student.UI.Models.Telegram;

namespace Gamification.Student.UI.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(WebAppInitData telegramAuthData);
    }
}
