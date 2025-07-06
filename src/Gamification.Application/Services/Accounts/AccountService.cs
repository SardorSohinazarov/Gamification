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

        public async Task<TokenDto> LoginAsync(WebAppUser telegramAuthData)
        {
            var isValidUser = _telegramHashVerifier
                .Validate(telegramAuthData.initData);

            if (!isValidUser.succes)
            {
                throw new UnauthorizedAccessException(isValidUser.error);
            }

            var user = await _gamificationDb.Users
                .FirstOrDefaultAsync(u => u.TelegramId == telegramAuthData.id);
            if (user == null)
            {
                user = new User
                {
                    TelegramId = telegramAuthData.id,
                    FirstName = telegramAuthData.first_name,
                    LastName = telegramAuthData.last_name,
                    Username = telegramAuthData.username,
                    LanguageCode = telegramAuthData.language_code,
                };
                var entry = await _gamificationDb.Users.AddAsync(user);
                await _gamificationDb.SaveChangesAsync();

                return await _tokenGenerator.GenerateTokenAsync(entry.Entity);
            }

            return await _tokenGenerator.GenerateTokenAsync(user);
        }
    }
}
