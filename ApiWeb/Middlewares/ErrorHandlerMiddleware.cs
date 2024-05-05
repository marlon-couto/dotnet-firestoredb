using ApiWeb.Exceptions;
using ApiWeb.Helpers;

namespace ApiWeb.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (UnauthorizedAccessException)
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            RespostaDaApi res = new() { Mensagem = "Acesso não autorizado." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (TaskCanceledException)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            RespostaDaApi res = new() { Mensagem = "Operação cancelada pelo usuário." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (NaoEncontradoException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            RespostaDaApi res = new() { Mensagem = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (Exception e)
        {
#if DEBUG
            Console.WriteLine(e.GetType());
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
#endif
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            RespostaDaApi res = new() { Mensagem = "Um erro aconteceu ao processar sua requisição." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
    }
}