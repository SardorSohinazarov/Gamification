using Common.ServiceAttribute;
using Gamification.Application.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Gamification.Application.Helpers
{
    [ScopedService]
    public class TelegramHashVerifier
    {
        private readonly IConfiguration _configuration;

        public TelegramHashVerifier(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool VerifyHash(TelegramAuthData data)
        {
            var authData = new SortedDictionary<string, string>
            {
                ["auth_date"] = data.Auth_date,
                ["id"] = data.User.Id.ToString()
            };

            if (!string.IsNullOrEmpty(data.User.First_name)) authData["first_name"] = data.User.First_name;
            if (!string.IsNullOrEmpty(data.User.Last_name)) authData["last_name"] = data.User.Last_name;
            if (!string.IsNullOrEmpty(data.User.Username)) authData["username"] = data.User.Username;
            if (!string.IsNullOrEmpty(data.User.Language_code)) authData["language_code"] = data.User.Language_code;

            if (!string.IsNullOrEmpty(data.Query_id)) authData["query_id"] = data.Query_id;
            if (!string.IsNullOrEmpty(data.Start_param)) authData["start_param"] = data.Start_param;

            var dataCheckString = string.Join("\n", authData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("WebAppData:" + _configuration["TelegramBot:Token"]));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
            var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return hex == data.Hash.ToLower();
        }
    }
}
