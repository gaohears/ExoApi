namespace ExoApi.Models;

// Formato padrão para operações realizadas com sucesso
public class SucessoResposta
{
    public SucessoResposta(int status, string mensagem, object? dados = null)
    {
        Status = status;
        Mensagem = mensagem;
        Dados = dados;
    }

    public int Status { get; set; }
    public string Mensagem { get; set; }
    public object? Dados { get; set; }
}