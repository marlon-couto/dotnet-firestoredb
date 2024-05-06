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
            var res = new RespostaDaApi { Mensagem = "Acesso não autorizado." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (OperationCanceledException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            var res = new RespostaDaApi { Mensagem = "Operação cancelada." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (NaoEncontradoException e)
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            var res = new RespostaDaApi { Mensagem = e.Message };
            await ctx.Response.WriteAsJsonAsync(res);
        }
        catch (Exception e)
        {
#if DEBUG
            ImprimirErroNoConsole(e);
#endif
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var res = new RespostaDaApi { Mensagem = "Um erro aconteceu ao processar sua requisição." };
            await ctx.Response.WriteAsJsonAsync(res);
        }
    }

    private static void ImprimirErroNoConsole(Exception excecao)
    {
        Console.Write("Tipo de exceção: ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(excecao.GetType());
        Console.ResetColor();
        Console.WriteLine("Erro: " + excecao.Message);
        Console.WriteLine("Stacktrace:");
        Console.WriteLine(excecao.StackTrace);
    }
}