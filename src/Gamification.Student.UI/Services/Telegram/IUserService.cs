using Gamification.Student.UI.Models;

namespace Gamification.Student.UI.Services.Telegram
{
    public interface IUserService
    {
        Task<TelegramUser> GetUserAsync();
    }
}
