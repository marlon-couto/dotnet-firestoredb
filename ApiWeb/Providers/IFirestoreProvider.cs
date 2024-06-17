using ApiWeb.Models;
using Google.Cloud.Firestore;

namespace ApiWeb.Providers;

public interface IFirestoreProvider
{
    /// <summary>
    ///     Cria ou atualiza uma entidade no Firestore. Caso a entidade exista, ela é atualizada. Caso não exista, ela é
    ///     criada.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="entidade">A entidade de tipo T que será criada ou atualizada.</param>
    /// <returns>Uma <see cref="Task" /> vazia.</returns>
    Task CriarOuAtualizar<T>(T entidade) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Atualiza uma entidade no Firestore.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="entidade">A entidade de tipo T que será atualizada.</param>
    /// <param name="doc">O documento que será atualizado.</param>
    /// <returns>Uma <see cref="Task" /> vazia.</returns>
    Task Atualizar<T>(T entidade, DocumentReference doc) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Busca um documento no Firestore.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="id">Uma string que representa o ID do documento.</param>
    /// <returns>Um <see cref="DocumentReference" /> representando o documento buscado.</returns>
    Task<DocumentReference> BuscarDocumento<T>(string id) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Busca um documento no Firestore.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="id">Um GUID que representa o ID do documento.</param>
    /// <returns>Um <see cref="DocumentReference" /> representando o documento buscado.</returns>
    Task<DocumentReference> BuscarDocumento<T>(Guid id) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Busca uma entidade no Firestore pelo ID.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="id">Uma string que representa o ID do documento.</param>
    /// <returns>Uma <see cref="IEntidadeDoFirebase" /> representando a entidade buscada.</returns>
    Task<T> BuscarPorId<T>(string id) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Busca uma entidade no Firestore pelo ID.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="id">Um GUID que representa o ID do documento.</param>
    /// <returns>Uma <see cref="IEntidadeDoFirebase" /> representando a entidade buscada.</returns>
    Task<T> BuscarPorId<T>(Guid id) where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Lista todas as entidades de uma coleção do Firestore. Os resultados são paginados.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="pagina">A página que será exibida. O padrão é 1.</param>
    /// <param name="quantidadePorPagina">A quantidade que será exibida por página. O padrão é 10.</param>
    /// <returns>
    ///     Uma tupla contendo <see cref="IReadOnlyCollection{T}" /> com o resultado da busca e a contagem do total de
    ///     documentos na coleção.
    /// </returns>
    Task<(List<T> docs, int contagem)> ListarTodos<T>(int pagina = 1, int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Lista as entidades de uma coleção usando um campo como filtro. As entidades retornadas são as que tem o valor
    ///     idêntico ao buscado. Os resultados são paginados.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="campo">O nome do campo que será usado como filtro.</param>
    /// <param name="valor">O valor do campo que será usado como filtro.</param>
    /// <param name="pagina">A página que será exibida. O padrão é 1.</param>
    /// <param name="quantidadePorPagina">A quantidade que será exibida por página. O padrão é 10.</param>
    /// <returns>
    ///     Uma tupla contendo <see cref="IReadOnlyCollection{T}" /> com o resultado da busca e a contagem do total de
    ///     documentos encontrados.
    /// </returns>
    Task<(List<T> docs, int contagem)> ListarIguais<T>(string campo
        , object valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase;

    /// <summary>
    ///     Lista as entidades de uma coleção usando um campo como filtro. As entidades retornadas são as que tem o valor
    ///     semelhante ao buscado. Os resultados são paginados.
    /// </summary>
    /// <typeparam name="T">Um tipo derivado de <see cref="IEntidadeDoFirebase" />.</typeparam>
    /// <param name="campo">O nome do campo que será usado como filtro.</param>
    /// <param name="valor">O valor do campo que será usado como filtro.</param>
    /// <param name="pagina">A página que será exibida. O padrão é 1.</param>
    /// <param name="quantidadePorPagina">A quantidade que será exibida por página. O padrão é 10.</param>
    /// <returns>
    ///     Uma tupla contendo <see cref="IReadOnlyCollection{T}" /> com o resultado da busca e a contagem do total de
    ///     documentos encontrados.
    /// </returns>
    Task<(List<T> docs, int contagem)> ListarSemelhantes<T>(string campo
        , string valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase;

    Task<(List<T> docs, int contagem)> ListarSemelhantesEmArray<T>(string campo
        , string valor
        , int pagina = 1
        , int quantidadePorPagina = 10)
        where T : IEntidadeDoFirebase;

    Task Excluir<T>(string id) where T : IEntidadeDoFirebase;
    Task Excluir<T>(Guid id) where T : IEntidadeDoFirebase;
    Task Excluir(DocumentReference doc);
}