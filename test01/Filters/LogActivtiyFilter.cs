using Azure.Core.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace test01.Filters
{
    public class LogActivtiyFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger<LogActivtiyFilter> logger;

        public LogActivtiyFilter(ILogger<LogActivtiyFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // ActionArguments is not available in ActionExecutedContext, so remove it from the log.
            logger.LogInformation(
                "Action {ActionName} executed by {Controller} executed at {ExecutedAt}",
                context.ActionDescriptor.DisplayName,
                context.Controller?.GetType().Name,
                DateTime.Now
            );
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Use structured logging to avoid CA2254
            logger.LogInformation(
                "Action {ActionName} executed by {Controller} at {ExecutedAt} with Arguments {@Arguments}",
                context.ActionDescriptor.DisplayName,
                context.Controller?.GetType().Name,
                DateTime.Now,
                JsonSerializer.Serialize(context.ActionArguments)
            );
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Use structured logging to avoid CA2254
            logger.LogInformation(
                "(async) Action {ActionName} executed by {Controller} at {ExecutedAt} with Arguments {@Arguments}",
                context.ActionDescriptor.DisplayName,
                context.Controller?.GetType().Name,
                DateTime.Now,
                JsonSerializer.Serialize(context.ActionArguments)
            );
            await next();
            // ActionArguments is not available in ActionExecutedContext, so remove it from the log.
            logger.LogInformation(
                "(async) Action {ActionName} executed by {Controller} executed at {ExecutedAt}",
                context.ActionDescriptor.DisplayName,
                context.Controller?.GetType().Name,
                DateTime.Now
            );
        }
    }
}
