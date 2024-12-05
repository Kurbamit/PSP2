using Microsoft.AspNetCore.Mvc.Filters;

namespace ReactApp1.Server.Filters;

public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;
    
    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Executing action {ActionName} with arguments {Arguments}",
            context.ActionDescriptor.DisplayName,
            context.ActionArguments);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Finished executing action {ActionName}",
            context.ActionDescriptor.DisplayName);
    }
}