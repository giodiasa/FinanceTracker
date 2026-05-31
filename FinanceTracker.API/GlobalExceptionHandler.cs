using FinanceTracker.Application.Errors;
using FinanceTracker.Application.Exceptions;
using System.Net;

namespace FinanceTracker.API
{
    internal sealed class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            RequestDelegate next,
            ILogger<GlobalExceptionHandler> logger)
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
            catch (AppException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Application error occurred. Code: {Code}, Message: {Message}",
                    ex.Code,
                    ex.Message);

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Code = ex.Code,
                    Message = ex.Message,
                    Details = ex.Details
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Code = "INTERNAL_SERVER_ERROR",
                    Message = "An unexpected error occurred",
                    Details = null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
