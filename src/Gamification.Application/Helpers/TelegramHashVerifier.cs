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
            var token = _configuration["TelegramBot:Token"];
            var secretKey = SHA256.HashData(Encoding.UTF8.GetBytes(token)); // ⚠️ bu juda muhim

            // Dictionary to string
            var fields = new List<string>
            {
                $"auth_date={data.Auth_date}",
                $"id={data.User.Id}"
            };

            if (!string.IsNullOrEmpty(data.User.First_name))
                fields.Add($"first_name={data.User.First_name}");
            if (!string.IsNullOrEmpty(data.User.Last_name))
                fields.Add($"last_name={data.User.Last_name}");
            if (!string.IsNullOrEmpty(data.User.Username))
                fields.Add($"username={data.User.Username}");
            if (!string.IsNullOrEmpty(data.User.Language_code))
                fields.Add($"language_code={data.User.Language_code}");
            if (!string.IsNullOrEmpty(data.Query_id))
                fields.Add($"query_id={data.Query_id}");
            if (!string.IsNullOrEmpty(data.Start_param))
                fields.Add($"start_param={data.Start_param}");

            fields.Sort(); // 🔐 muhim: alfavit bo‘yicha
            var dataCheckString = string.Join("\n", fields);

            // HMAC-SHA256 orqali hash hisoblash
            using var hmac = new HMACSHA256(secretKey);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
            var computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            // Debug uchun log qilish
            Console.WriteLine("dataCheckString:\n" + dataCheckString);
            Console.WriteLine("expected hash: " + data.Hash.ToLower());
            Console.WriteLine("computed hash: " + computedHash);

            return computedHash == data.Hash.ToLowerInvariant();
        }


        //public bool VerifyHash(TelegramAuthData data)
        //{
        //    var authData = new SortedDictionary<string, string>
        //    {
        //        ["auth_date"] = data.Auth_date,
        //        ["id"] = data.User.Id.ToString()
        //    };

        //    if (!string.IsNullOrEmpty(data.User.First_name)) authData["first_name"] = data.User.First_name;
        //    if (!string.IsNullOrEmpty(data.User.Last_name)) authData["last_name"] = data.User.Last_name;
        //    if (!string.IsNullOrEmpty(data.User.Username)) authData["username"] = data.User.Username;
        //    if (!string.IsNullOrEmpty(data.User.Language_code)) authData["language_code"] = data.User.Language_code;

        //    if (!string.IsNullOrEmpty(data.Query_id)) authData["query_id"] = data.Query_id;
        //    if (!string.IsNullOrEmpty(data.Start_param)) authData["start_param"] = data.Start_param;

        //    var dataCheckString = string.Join("\n", authData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        //    var token = _configuration["TelegramBot:Token"];

        //    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("WebAppData:" + token));
        //    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataCheckString));
        //    var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();

        //    return hex == data.Hash.ToLower();
        //}
    }
}
