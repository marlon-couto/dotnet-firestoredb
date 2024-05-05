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
        int porta = HttpContext.Connection.LocalPort;
        RespostaDaApi res = new() { Mensagem = $"O servidor está rodando na porta {porta}.", Sucesso = true };
        return Ok(res);
    }
}