using ClosureServices.Application.DTO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ClosureServices.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;
            var userId = context.User?.FindFirst("userId")?.Value ?? "Anonymous";
            var controller = context.Request.RouteValues["controller"]?.ToString() ?? "N/A";
            var action = context.Request.RouteValues["action"]?.ToString() ?? "N/A";

            try
            {
                await next(context);
                stopwatch.Stop();

                var log = BuildLog(method, path, controller, action, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, userId, null);
                logger.LogInformation(log);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                int statusCode = GetStatusCode(ex);
                string errorCode = ex.GetType().Name;

                var log = BuildLog(method, path, controller, action, statusCode, stopwatch.ElapsedMilliseconds, userId, ex.Message);
                logger.LogError(ex, log);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = ApiResponse<object>.ErrorResponse(ex.Message, errorCode, ex.Message);
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }

        private int GetStatusCode(Exception ex)
        {
            if (ex is UnauthorizedAccessException) return 401;
            if (ex is KeyNotFoundException) return 404;
            if (ex is InvalidOperationException) return 400;
            if (ex is ArgumentException) return 400;
            return 500;
        }

        private string BuildLog(string method, string path, string controller, string action, int status, long time, string user, string? error)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"{method} {path}");
            sb.AppendLine($"Controller: {controller}.{action}");
            sb.AppendLine($"Status: {status} | Time: {time}ms | User: {user}");

            if (!string.IsNullOrEmpty(error))
            {
                sb.AppendLine($"Error: {error}");
            }

            return sb.ToString();
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
