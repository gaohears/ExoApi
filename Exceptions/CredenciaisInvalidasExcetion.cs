namespace ExoApi.Exceptions;

// Será convertida em HTTP 401
public class CredenciaisInvalidasException : Exception
{
    public CredenciaisInvalidasException(string mensagem) : base(mensagem) { }
}