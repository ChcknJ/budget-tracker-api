using Microsoft.AspNetCore.Diagnostics;

namespace BudgetTracker.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Something went wrong.");

            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                status = 500,
                message = "An unexpected error occurred."
            });

            return true;
        }
    }
}
