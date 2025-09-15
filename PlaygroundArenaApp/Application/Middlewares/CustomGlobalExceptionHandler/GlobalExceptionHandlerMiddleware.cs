using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace PlaygroundArenaApp.Application.Middlewares.CustomGlobalExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var message = exception.Message;
            var StatusCode = 500;

            if(exception is KeyNotFoundException)
            {
                message = "No Data Registered!";
                StatusCode = 404;
            }

            if (exception is BadHttpRequestException)
            {
                message = "Invalid Data!";
                StatusCode = 400;
            }

            if (exception is UnauthorizedAccessException)
            {
                message = "Unauthorized access!";
                StatusCode = 401;
            }

            if (exception is ArgumentNullException)
            {
                message = "You cannot send null data!";
                StatusCode = 400;
            }

            if (exception is SqlException)
            {
                message = "Something went wrong with database!";
                StatusCode = 500;
            }

            var response = new ErrorResponse
            {
                Status = StatusCode,
                Message = message,
                Details = exception.Message
            };


            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;

        }
    }
}
