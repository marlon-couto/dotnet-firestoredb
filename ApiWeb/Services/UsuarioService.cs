using ApiWeb.Dtos;
using ApiWeb.Models;
using ApiWeb.Providers;
using System.Globalization;

namespace ApiWeb.Services;

public class UsuarioService(IFirestoreProvider fs) : IUsuarioService
{
    public async Task<Usuario> CriarUsuario(CriarUsuarioDto dto)
    {
        var usuario = new Usuario { Nome = dto.Nome };
        await fs.CriarOuAtualizar(usuario);
        return usuario;
    }

    public async Task<(List<Usuario> usuarios, int contagem)> ListarTodosUsuarios(int pagina, int quantidadePorPagina)
    {
        return await fs.ListarTodos<Usuario>(pagina, quantidadePorPagina);
    }

    public async Task<Usuario> BuscarUsuarioPorId(Guid usuarioId)
    {
        return await fs.BuscarPorId<Usuario>(usuarioId);
    }

    public async Task<(List<Usuario> usuariosEncontrados, int contagem)> BuscarUsuariosPorNome(string termoDeBusca
        , int pagina
        , int quantidadePorPagina)
    {
        var termoFormatado = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(termoDeBusca.ToLower());
        return await fs.ListarSemelhantes<Usuario>(nameof(Usuario.Nome), termoFormatado, pagina, quantidadePorPagina);
    }

    public async Task<Usuario> AtualizarUsuario(Guid usuarioId, AtualizarUsuarioDto dto)
    {
        var usuario = await fs.BuscarPorId<Usuario>(usuarioId);
        var usuarioAtualizado = new Usuario { Id = usuarioId.ToString("N"), Nome = dto.Nome ?? usuario.Nome };
        await fs.CriarOuAtualizar(usuarioAtualizado);
        return usuarioAtualizado;
    }

    public async Task ExcluirUsuario(Guid usuarioId)
    {
        var doc = await fs.BuscarDocumento<Usuario>(usuarioId);
        await fs.Excluir(doc);
    }
}