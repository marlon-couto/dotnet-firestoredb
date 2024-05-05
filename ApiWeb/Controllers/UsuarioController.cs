using ApiWeb.Dtos;
using ApiWeb.Exceptions;
using ApiWeb.Helpers;
using ApiWeb.Models;
using ApiWeb.Providers;
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
        return Created(string.Empty, res);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodosUsuarios(CancellationToken ct
        , [FromQuery] int pagina = 1
        , [FromQuery] int quantidade = 10)
    {
        IReadOnlyCollection<Usuario> usuarios = await fs.ListarTodos<Usuario>(pagina, quantidade, ct);
        RespostaDaApi<IReadOnlyCollection<Usuario>> res = new()
        {
            Dados = usuarios, Mensagem = "Usuários listados com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpGet("{usuarioId:guid}")]
    public async Task<IActionResult> BuscarUsuarioPorId([FromRoute] Guid usuarioId, CancellationToken ct)
    {
        Usuario usuario = await fs.BuscarPorId<Usuario>(usuarioId.ToString("N"), ct);
        if (usuario == null)
        {
            throw new NaoEncontradoException("Usuário não encontrado.");
        }

        RespostaDaApi<Usuario> res = new()
        {
            Dados = usuario, Mensagem = "Usuário encontrado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarUsuarioPorNome([FromQuery(Name = "nome")] string termoDeBusca
        , CancellationToken ct)
    {
        string termoFormatado
            = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(termoDeBusca.Replace('%', ' ').ToLower());
        IReadOnlyCollection<Usuario> usuarios
            = await fs.ListarIguaisA<Usuario>(nameof(Usuario.Nome), termoFormatado, ct);
        if (usuarios.Count == 0)
        {
            throw new NaoEncontradoException("Nenhum usuário encontrado com esse nome.");
        }

        RespostaDaApi<IReadOnlyCollection<Usuario>> res = new()
        {
            Dados = usuarios, Mensagem = "Usuário encontrado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpPut("{usuarioId:guid}")]
    public async Task<IActionResult> AtualizarUsuario([FromRoute] Guid usuarioId
        , [FromBody] AtualizarUsuarioDto dto
        , CancellationToken ct)
    {
        Usuario usuario = await fs.BuscarPorId<Usuario>(usuarioId, ct);
        if (usuario == null)
        {
            throw new NaoEncontradoException("Usuário não encontrado.");
        }

        Usuario usuarioAtualizado = new(usuarioId, dto.Nome);
        await fs.CriarOuAtualizar(usuarioAtualizado, ct);
        RespostaDaApi<Usuario> res = new()
        {
            Dados = usuarioAtualizado, Mensagem = "Usuário atualizado com sucesso.", Sucesso = true
        };
        return Ok(res);
    }

    [HttpDelete("{usuarioId:guid}")]
    public async Task<IActionResult> ExcluirUsuario([FromRoute] Guid usuarioId, CancellationToken ct)
    {
        Usuario usuario = await fs.BuscarPorId<Usuario>(usuarioId, ct);
        if (usuario == null)
        {
            throw new NaoEncontradoException("Usuário não encontrado.");
        }

        await fs.Excluir<Usuario>(usuarioId, ct);
        RespostaDaApi res = new() { Mensagem = "Usuário excluído com sucesso.", Sucesso = true };
        return Ok(res);
    }
}