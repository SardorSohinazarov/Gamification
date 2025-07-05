using Common.ServiceAttribute;
using Gamification.Application.DataTransferObjects.Auth;
using Gamification.Application.Helpers;
using Gamification.Application.Models;
using Gamification.Domain.Entities;
using Gamification.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Application.Services.Accounts
{
    [ScopedService]
    public class AccountService : IAccountService
    {
        private readonly TelegramHashVerifier _telegramHashVerifier;
        private readonly GamificationDb _gamificationDb;
        private readonly TokenGenerator _tokenGenerator;

        public AccountService(
            TelegramHashVerifier telegramHashVerifier,
            GamificationDb gamificationDb,
            TokenGenerator tokenGenerator)
        {
            _telegramHashVerifier = telegramHashVerifier;
            _gamificationDb = gamificationDb;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenDto> LoginAsync(TelegramAuthData telegramAuthData)
        {
            var isValidUser = _telegramHashVerifier
                .VerifyHash(telegramAuthData);

            if (!isValidUser)
            {
                throw new UnauthorizedAccessException("Invalid Telegram authentication data.");
            }

            var user = await _gamificationDb.Users
                .FirstOrDefaultAsync(u => u.TelegramId == telegramAuthData.User.Id);
            if (user == null)
            {
                user = new User
                {
                    TelegramId = telegramAuthData.User.Id,
                    FirstName = telegramAuthData.User.First_name,
                    LastName = telegramAuthData.User.Last_name,
                    Username = telegramAuthData.User.Username,
                    LanguageCode = telegramAuthData.User.Language_code,
                };
                var entry = await _gamificationDb.Users.AddAsync(user);
                await _gamificationDb.SaveChangesAsync();

                return await _tokenGenerator.GenerateTokenAsync(entry.Entity);
            }

            return await _tokenGenerator.GenerateTokenAsync(user);
        }
    }
}
