using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace PlaygroundArenaApp.Application.Middlewares.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();
            var executedContext = await next();
            sw.Stop();

            var time = sw.ElapsedMilliseconds;
            _logger.LogInformation("{Action} took {Ms}ms", context.ActionDescriptor.DisplayName, time);
        }
    }
}
