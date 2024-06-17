using ApiWeb.Dtos;
using ApiWeb.Models;

namespace ApiWeb.Services;

public interface IUsuarioService
{
    Task<Usuario> CriarUsuario(CriarUsuarioDto dto);
    Task<(List<Usuario> usuarios, int contagem)> ListarTodosUsuarios(int pagina, int quantidadePorPagina);
    Task<Usuario> BuscarUsuarioPorId(Guid usuarioId);

    Task<(List<Usuario> usuariosEncontrados, int contagem)> BuscarUsuariosPorNome(string termoDeBusca
        , int pagina
        , int quantidadePorPagina);

    Task<Usuario> AtualizarUsuario(Guid usuarioId, AtualizarUsuarioDto dto);
    Task ExcluirUsuario(Guid usuarioId);
}