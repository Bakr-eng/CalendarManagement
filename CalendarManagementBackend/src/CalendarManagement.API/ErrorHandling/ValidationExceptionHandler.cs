using CalendarManagement.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CalendarManagement.API.ErrorHandling
{
    public class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not RequestValidationException validationException)
            {
                return false;
            }

            var result = TypedResults.ValidationProblem(validationException.Errors);
            await result.ExecuteAsync(httpContext);

            return true;
        }
    }
}