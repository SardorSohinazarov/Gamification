using Common.ServiceAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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

        [HttpPost("validate")]
        public (bool succes, string error) Validate(string initData)
        {
            // Parse string initData from telegram.
            var data = HttpUtility.ParseQueryString(initData);

            // Put data in a alphabetically sorted dict.
            var dataDict = new SortedDictionary<string, string>(
                data.AllKeys.ToDictionary(x => x!, x => data[x]!),
                StringComparer.Ordinal);

            // https://core.telegram.org/bots/webapps#validating-data-received-via-the-web-app:
            // Data-check-string is a chain of all received fields,
            // sorted alphabetically.
            // in the format key=<value>.
            // with a line feed character ('\n', 0x0A) used as separator.
            // e.g., 'auth_date=<auth_date>\nquery_id=<query_id>\nuser=<user>'
            var dataCheckString = string.Join(
                '\n', dataDict.Where(x => x.Key != "hash") // Hash should be removed.
                    .Select(x => $"{x.Key}={x.Value}")); // like auth_date=<auth_date> ..

            // secrecKey is the HMAC-SHA-256 signature of the bot's token
            // with the constant string WebAppData used as a key.

            var token = _configuration["TelegramBot:Token"];
            var constantKey = "WebAppData";

            var secretKey = ComputeHMACSHA256(
                Encoding.UTF8.GetBytes(constantKey), // WebAppData
                Encoding.UTF8.GetBytes(token)); // Bot's token

            var generatedHash = ComputeHMACSHA256(
                secretKey,
                Encoding.UTF8.GetBytes(dataCheckString)); // data_check_string

            // Convert received hash from telegram to a byte array.
            var actualHash = Convert.FromHexString(dataDict["hash"]); // .NET 5.0 

            // Compare our hash with the one from telegram.
            if (actualHash.SequenceEqual(generatedHash))
            {
                // Optionally, check the auth_date to prevent outdated data
                if (dataDict.TryGetValue("auth_date", out var authDateStr) && long.TryParse(authDateStr, out var authDate))
                {
                    var authDateTime = DateTimeOffset.FromUnixTimeSeconds(authDate);
                    if (authDateTime < DateTimeOffset.UtcNow.AddMinutes(-5))
                    {
                        return (false, "Auth date is too old");
                    }
                }

                return (true,"Ok");
            }

            return (false,"Invalid data");
        }

        private static byte[] ComputeHMACSHA256(byte[] key, byte[] data)
        {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(data);
        }
    }
}
