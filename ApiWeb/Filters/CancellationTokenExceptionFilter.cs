using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiWeb.Filters;

public class CancellationTokenExceptionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
    {
        var cancellationToken = ctx.HttpContext.RequestAborted;
        try
        {
            await next();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("A operação foi cancelada pelo usuário.");
        }
        catch (OperationCanceledException)
        {
            throw new OperationCanceledException("A operação foi cancelada pelo servidor.");
        }
    }
}