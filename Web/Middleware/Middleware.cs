using System.Net;
using System.Text.Json;

namespace Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, $"Missing matrix: {ex.ParamName}", HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, $"Matrix error: {ex.Message}", HttpStatusCode.BadRequest);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, $"Invalid argument: {ex.Message}", HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                await HandleExceptionAsync(context, "One of the required matrix values is null.", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred");
                await HandleExceptionAsync(context, "Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new
            {
                status = context.Response.StatusCode,
                error = message
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
