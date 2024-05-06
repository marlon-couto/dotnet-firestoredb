namespace ApiWeb.Helpers;

public class RespostaDaApi
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}

public class RespostaDaApi<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public T? Dados { get; set; }
}

public class RespostaDaApiPaginada<T> : RespostaDaApi<T>
{
    public Paginacao Paginacao { get; set; } = null!;
}

public class Paginacao
{
    public int Pagina { get; set; }
    public int QuantidadePorPagina { get; set; }
    public int? Total { get; set; }
}