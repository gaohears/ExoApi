namespace ExoApi.Exceptions;

// Será convertida em HTTP 404
public class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string mensagem) : base(mensagem) { }
}