using Gamification.Application.DataTransferObjects.Auth;
using Gamification.Application.Models;

namespace Gamification.Application.Services.Accounts
{
    public interface IAccountService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> LoginAsync(TelegramAuthData telegramAuthData);
    }
}
