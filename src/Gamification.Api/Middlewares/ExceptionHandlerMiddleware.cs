using System.Net;
using Common;
using System.Text.Json;
using Gamification.Application.Helpers;

namespace Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelegramBotHelper _telegramBotHelper;
        public ExceptionHandlingMiddleware(RequestDelegate next, TelegramBotHelper telegramBotHelper)
        {
            _next = next;
            _telegramBotHelper = telegramBotHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex, "Resource not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex, "Unauthorized access");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, Exception ex, string errorMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var result = Result.Fail(errorMessage);
            var jsonResult = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(jsonResult);
            await _telegramBotHelper.SendExceptionAsync(ex, context);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}