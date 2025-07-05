using Common.ServiceAttribute;
using Gamification.Application.DataTransferObjects.Auth;
using Gamification.Domain.Entities;
using Gamification.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gamification.Application.Helpers
{
    [ScopedService]
    public class TokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly GamificationDb _gamificationDb;
        public TokenGenerator(
            IConfiguration configuration, GamificationDb gamificationDb)
        {
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            _gamificationDb = gamificationDb;
        }

        public async Task<TokenDto> GenerateTokenAsync(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes);
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // Agar kerak bo'lsa, o'zgartiring
                audience: _jwtSettings.Audience, // Agar kerak bo'lsa, o'zgartiring
                claims: claims,
                expires: accessTokenExpiration,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDto
            {
                AccessToken = tokenString,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireDate = refreshTokenExpiration
            };
        }

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid access token.");

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("Invalid token claims.");

            var user = await _gamificationDb.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpireDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (user == null)
                throw new Exception("User not found.");

            return await GenerateTokenAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
