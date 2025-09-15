using System.Diagnostics;

namespace PlaygroundArenaApp.Application.Middlewares.CustomSerilogLogging
{
    public class SerilogLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SerilogLoggingMiddleware> _logger;

        public SerilogLoggingMiddleware(RequestDelegate next, ILogger<SerilogLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        //Custom MiddleWare Function
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("-> Incoming Request: {Method} {Path}", context.Request.Method, context.Request.Path);

                await _next(context);

                _logger.LogInformation("<- Outgoing Response: {StatusCode} at {Time}", context.Response.StatusCode, DateTime.UtcNow);
            }
            catch (Exception ex){
                _logger.LogError(ex, " Exception for {Method} {Path} at {Time}",
                context.Request.Method,
                context.Request.Path,
                DateTime.UtcNow);

                //So that next GLobal Handles catches it
                throw;
            }
        }

    }
}
