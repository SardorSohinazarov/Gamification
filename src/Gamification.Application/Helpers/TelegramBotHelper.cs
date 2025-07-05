using Common.ServiceAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Gamification.Application.Helpers
{
    [SingletonService]
    public class TelegramBotHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TelegramBotHelper> _logger;

        public TelegramBotHelper(IConfiguration configuration, ILogger<TelegramBotHelper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Exceptionni berish orqali Telegramga xabar jo'natadi
        /// </summary>
        /// <param name="exMessage"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task SendExceptionAsync(Exception exMessage, HttpContext httpContext)
        {
            var request = httpContext?.Request;
            var host = httpContext?.Request?.Host;
            string hosturl = host is null ? "" : host.Value.Host.ToString();
            var query = request?.Query;
            string queryString = string.Join("&", query?.Select(q => $"{q.Key}={q.Value}"));
            string userId = httpContext?.User?.FindFirst("UserId")?.Value;
            string body = string.Empty;
            if (httpContext?.Request?.Method != "GET")
                body = await ReadRequestBodyIfJsonAsync(httpContext);

            var message = $"⚠ Host: {hosturl} failed" +
                          $"\n🚨 URL: {request?.Method} {request?.Path}\n" +
                          (userId is not null ? $"\n👤 UserID: {userId}" : string.Empty) +
                          (!string.IsNullOrWhiteSpace(queryString) ? $"\n🔗 Query: {queryString}" : string.Empty) +
                          (!string.IsNullOrWhiteSpace(body) ? $"\n📄 Body: {body}" : string.Empty) +
                          $"\n❗ Exception: {exMessage.Message}\n" +
                          $"\n🧵 StackTrace: {exMessage.StackTrace}\n" +
                          $"\n🕒 Moment: {DateTime.UtcNow}";

            var _errorBotToken = _configuration["TelegramBot:Token"];
            var _errorChatId = _configuration["TelegramBot:ChatId"];

            await SendTextAsync(_errorBotToken, _errorChatId, message);
        }

        /// <summary>
        /// Telegramga xabar jo'natadi
        /// </summary>
        /// <param name="botToken"></param>
        /// <param name="chatId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendTextAsync(string botToken, string chatId, string message)
        {
            var maxMessageLength = 4096;
            using var httpClient = new HttpClient();

            // Agar xabar uzun bo‘lsa, bo‘laklarga bo‘lamiz
            for (int i = 0; i < message.Length; i += maxMessageLength)
            {
                var part = message.Substring(i, Math.Min(maxMessageLength, message.Length - i));

                var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    chat_id = chatId,
                    text = part,
                }), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Failed to send message to Telegram: {errorMessage}");
                }
            }
        }

        private async Task<string> ReadRequestBodyIfJsonAsync(HttpContext context)
        {
            context.Request.EnableBuffering(); // allows re-reading the body

            var contentType = context.Request.ContentType?.ToLower();

            if (string.IsNullOrEmpty(contentType) || !contentType.Contains("application/json"))
                return "[Skipped: Non-JSON Content-Type]";

            if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
                return "[Empty Body]";

            context.Request.Body.Position = 0;

            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body;
        }
    }
}
