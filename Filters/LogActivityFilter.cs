using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters;

// we should extend one of those IActionFilter, IAsyncActionFilter 
public class LogActivityFilter(ILogger<LogActivityFilter> logger) : IActionFilter, IAsyncActionFilter
{
    private readonly ILogger<LogActivityFilter> _logger = logger;


    public void OnActionExecuting(ActionExecutingContext context)
    {
        // context.Result = new NotFoundResult(); // short circuit
        _logger.LogInformation(
            "Executing action: #{context.ActionDescriptor.DisplayName}"
            + "on Controller: #{context.Controller}"
            + "with Arguments: #{JsonSerializer.Serialize(context.ActionArguments)}",
            context.ActionDescriptor.DisplayName,
            context.Controller,
            JsonSerializer.Serialize(context.ActionArguments)
        );
    }


    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation(
            "Action: #{context.ActionDescriptor.DisplayName}"
            + "Executed on Controller: #{context.Controller}",
            context.ActionDescriptor.DisplayName,
            context.Controller
        );
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation(
            "(Async) Executing action: #{context.ActionDescriptor.DisplayName}"
            + "on Controller: #{context.Controller}"
            + "with Arguments: #{JsonSerializer.Serialize(context.ActionArguments)}",
            context.ActionDescriptor.DisplayName,
            context.Controller,
            JsonSerializer.Serialize(context.ActionArguments)
        );

        await next();

        _logger.LogInformation(
            "(Async) Action: #{context.ActionDescriptor.DisplayName}"
            + "Executed on Controller: #{context.Controller}",
            context.ActionDescriptor.DisplayName,
            context.Controller
        );
    }
}