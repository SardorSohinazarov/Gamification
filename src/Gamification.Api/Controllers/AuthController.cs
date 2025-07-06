using Common;
using Gamification.Application.DataTransferObjects.Auth;
using Gamification.Application.Models;
using Gamification.Application.Services.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("telegram")]
        public async Task<Result<TokenDto>> Login([FromBody] WebAppUser telegramAuthData)
        {
            var token = await _accountService.LoginAsync(telegramAuthData);
            return Result<TokenDto>.Success(token);
        }
    }
}
