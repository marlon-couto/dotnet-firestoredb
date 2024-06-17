using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto dto)
    {
        var novoUsuario = await usuarioService.CriarUsuario(dto);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = novoUsuario, Mensagem = "Usuário criado com sucesso.", Sucesso = true
        };

        return Created($"api/usuario/{novoUsuario.Id}", res);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosUsuarios([FromQuery] int pagina = 1
        , [FromQuery(Name = "quantidade")] int quantidadePorPagina = 10)
    {
        var (usuarios, contagem) = await usuarioService.ListarTodosUsuarios(pagina, quantidadePorPagina);
        var res = new RespostaDaApiPaginada<List<Usuario>>
        {
            Dados = usuarios
            , Mensagem = "Usuários listados com sucesso."
            , Sucesso = true
            , Paginacao = new Paginacao
            {
                Pagina = pagina, QuantidadePorPagina = quantidadePorPagina, Total = contagem
            }
        };

        return Ok(res);
    }

    [HttpGet("{usuarioId:guid}")]
    public async Task<IActionResult> BuscarUsuarioPorId([FromRoute] Guid usuarioId)
    {
        var usuarioEncontrado = await usuarioService.BuscarUsuarioPorId(usuarioId);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = usuarioEncontrado, Mensagem = "Usuário encontrado com sucesso.", Sucesso = true
        };

        return Ok(res);
    }

    [HttpGet("buscar-nome/{termoDeBusca}")]
    public async Task<IActionResult> BuscarUsuariosPorNome([FromRoute] string termoDeBusca
        , [FromQuery] int pagina = 1
        , [FromQuery(Name = "quantidade")] int quantidadePorPagina = 10)
    {
        var (usuariosEncontrados, contagem)
            = await usuarioService.BuscarUsuariosPorNome(termoDeBusca, pagina, quantidadePorPagina);

        var res = new RespostaDaApiPaginada<List<Usuario>>
        {
            Dados = usuariosEncontrados
            , Mensagem = "Usuários encontrados com sucesso."
            , Sucesso = true
            , Paginacao = new Paginacao
            {
                Pagina = pagina, QuantidadePorPagina = quantidadePorPagina, Total = contagem
            }
        };

        return Ok(res);
    }

    [HttpPut("{usuarioId:guid}")]
    public async Task<IActionResult> AtualizarUsuario([FromRoute] Guid usuarioId, [FromBody] AtualizarUsuarioDto dto)
    {
        var usuarioAtualizado = await usuarioService.AtualizarUsuario(usuarioId, dto);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = usuarioAtualizado, Mensagem = "Usuário atualizado com sucesso.", Sucesso = true
        };

        return Ok(res);
    }

    [HttpDelete("{usuarioId:guid}")]
    public async Task<IActionResult> ExcluirUsuario([FromRoute] Guid usuarioId)
    {
        await usuarioService.ExcluirUsuario(usuarioId);
        var res = new RespostaDaApi { Mensagem = "Usuário excluído com sucesso.", Sucesso = true };
        return Ok(res);
    }
}