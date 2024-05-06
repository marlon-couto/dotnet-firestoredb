using ApiWeb.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TesteController : ControllerBase
{
    [HttpGet("servidor")]
    public IActionResult TestarServidor()
    {
        var porta = HttpContext.Connection.LocalPort;
        var res = new RespostaDaApi { Mensagem = $"O servidor está rodando na porta {porta}.", Sucesso = true };
        return Ok(res);
    }
}