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