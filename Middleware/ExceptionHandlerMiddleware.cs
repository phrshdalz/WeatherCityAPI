using System.Net;
using System.Text.Json;

namespace WeatherCityAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = exception.Message;

       
            if (exception.Message.Contains("400"))
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception.Message.Contains("401"))
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
            }
            else if (exception.Message.Contains("404"))
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception.Message.Contains("429"))
            {
                statusCode = (HttpStatusCode)429;
                message = exception.Message;
            }
            else if (exception.Message.Contains("5xx") || exception.Message.Contains("500"))
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = new
                {
                    message = message,
                    details = exception.InnerException?.Message,
                    stackTrace = GetStackTrace(exception)
                }
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }

        private static string? GetStackTrace(Exception exception)
        {
                return exception.StackTrace;    
        }
    }

    // Extension method to make it easy to use in Program.cs
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
