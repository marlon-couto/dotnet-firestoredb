using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using ApiWeb.Providers;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(FirestoreProvider fs) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto dto, CancellationToken ct)
    {
        Usuario usuario = new(dto.Nome);
        await fs.CriarOuAtualizar(usuario, ct);
        RespostaDaApi<Usuario> res = new()
        {
            Dados = usuario, Mensagem = "Usuário criado com sucesso.", Sucesso = true
        };
        return Created($"api/usuario/{usuario.Id}", res);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosUsuarios(CancellationToken ct
        , [FromQuery] int pagina = 1
        , [FromQuery] int quantidade = 10)
    {
        Paginacao paginacao = new() { Pagina = pagina, QuantidadePorPagina = quantidade };
        IReadOnlyCollection<Usuario> usuarios = await fs.ListarTodos<Usuario>(paginacao, ct);
        RespostaDaApiPaginada<IReadOnlyCollection<Usuario>> res = new()
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
    public async Task<IActionResult> BuscarUsuarioPorId([FromRoute] Guid usuarioId, CancellationToken ct)
    {
        Usuario usuario = await fs.BuscarPorId<Usuario>(usuarioId.ToString("N"), ct);
        RespostaDaApi<Usuario> res = new()
        {
            Dados = usuario, Mensagem = "Usuário encontrado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpGet("buscar-nome/{termoDeBusca}")]
    public async Task<IActionResult> BuscarUsuariosPorNome([FromRoute] string termoDeBusca
        , CancellationToken ct
        , [FromQuery] int pagina = 1
        , [FromQuery] int quantidade = 10)
    {
        string termoFormatado
            = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(termoDeBusca.Replace('%', ' ').ToLower());
        BuscaDeEntidadesPorCampoDto buscaDto = new()
        {
            Campo = nameof(Usuario.Nome)
            , Valor = termoFormatado
            , Paginacao = new Paginacao { Pagina = pagina, QuantidadePorPagina = quantidade }
        };
        IReadOnlyCollection<Usuario> usuarios = await fs.ListarSemelhantes<Usuario>(buscaDto, ct);
        RespostaDaApiPaginada<IReadOnlyCollection<Usuario>> res = new()
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
    public async Task<IActionResult> AtualizarUsuario([FromRoute] Guid usuarioId
        , [FromBody] AtualizarUsuarioDto dto
        , CancellationToken ct)
    {
        DocumentReference doc = await fs.BuscarDocumento<Usuario>(usuarioId, ct);
        Usuario usuarioAtualizado = new(usuarioId, dto.Nome);
        await fs.CriarOuAtualizar(usuarioAtualizado, doc, ct);
        RespostaDaApi<Usuario> res = new()
        {
            Dados = usuarioAtualizado, Mensagem = "Usuário atualizado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpDelete("{usuarioId:guid}")]
    public async Task<IActionResult> ExcluirUsuario([FromRoute] Guid usuarioId, CancellationToken ct)
    {
        DocumentReference doc = await fs.BuscarDocumento<Usuario>(usuarioId, ct);
        await fs.Excluir(doc, ct);
        RespostaDaApi res = new() { Mensagem = "Usuário excluído com sucesso.", Sucesso = true };
        return Ok(res);
    }
}