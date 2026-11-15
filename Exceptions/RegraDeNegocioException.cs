namespace ExoApi.Exceptions;

// Será convertida em HTTP 400
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem) { }
}