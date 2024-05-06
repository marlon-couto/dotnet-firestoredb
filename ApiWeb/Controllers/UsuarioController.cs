using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using ApiWeb.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(IFirestoreProvider fs) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto dto)
    {
        var usuario = new Usuario(dto.Nome);
        await fs.CriarOuAtualizar(usuario);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = usuario, Mensagem = "Usuário criado com sucesso.", Sucesso = true
        };
        return Created($"api/usuario/{usuario.Id}", res);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosUsuarios([FromQuery] int pagina = 1, [FromQuery] int quantidade = 10)
    {
        var paginacao = new Paginacao { Pagina = pagina, QuantidadePorPagina = quantidade };
        var usuarios = await fs.ListarTodos<Usuario>(paginacao);
        var res = new RespostaDaApiPaginada<IReadOnlyCollection<Usuario>>
        {
            Dados = usuarios
            , Mensagem = "Usuários listados com sucesso."
            , Sucesso = true
            , Paginacao = new Paginacao
            {
                Pagina = pagina, QuantidadePorPagina = quantidade, Total = usuarios.Count
            }
        };
        return Ok(res);
    }

    [HttpGet("{usuarioId:guid}")]
    public async Task<IActionResult> BuscarUsuarioPorId([FromRoute] Guid usuarioId)
    {
        var usuario = await fs.BuscarPorId<Usuario>(usuarioId);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = usuario, Mensagem = "Usuário encontrado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpGet("buscar-nome/{termoDeBusca}")]
    public async Task<IActionResult> BuscarUsuariosPorNome([FromRoute] string termoDeBusca
        , [FromQuery] int pagina = 1
        , [FromQuery] int quantidade = 10)
    {
        var termoFormatado
            = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(termoDeBusca.Replace('%', ' ').ToLower());
        var buscaDto = new BuscaDeEntidadesPorCampoDto
        {
            Campo = nameof(Usuario.Nome)
            , Valor = termoFormatado
            , Paginacao = new Paginacao { Pagina = pagina, QuantidadePorPagina = quantidade }
        };
        var usuarios = await fs.ListarSemelhantes<Usuario>(buscaDto);
        var res = new RespostaDaApiPaginada<IReadOnlyCollection<Usuario>>
        {
            Dados = usuarios
            , Mensagem = "Usuários encontrados com sucesso."
            , Sucesso = true
            , Paginacao = new Paginacao
            {
                Pagina = pagina, QuantidadePorPagina = quantidade, Total = usuarios.Count
            }
        };
        return Ok(res);
    }

    [HttpPut("{usuarioId:guid}")]
    public async Task<IActionResult> AtualizarUsuario([FromRoute] Guid usuarioId, [FromBody] AtualizarUsuarioDto dto)
    {
        var doc = await fs.BuscarDocumento<Usuario>(usuarioId);
        var usuarioAtualizado = new Usuario(usuarioId, dto.Nome);
        await fs.CriarOuAtualizar(usuarioAtualizado, doc);
        var res = new RespostaDaApi<Usuario>
        {
            Dados = usuarioAtualizado, Mensagem = "Usuário atualizado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpDelete("{usuarioId:guid}")]
    public async Task<IActionResult> ExcluirUsuario([FromRoute] Guid usuarioId)
    {
        var doc = await fs.BuscarDocumento<Usuario>(usuarioId);
        await fs.Excluir(doc);
        var res = new RespostaDaApi { Mensagem = "Usuário excluído com sucesso.", Sucesso = true };
        return Ok(res);
    }
}