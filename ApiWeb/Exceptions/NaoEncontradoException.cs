namespace ApiWeb.Exceptions;

public class NaoEncontradoException : Exception
{
    public NaoEncontradoException() : base("Entidade não encontrada.")
    {
    }

    public NaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}