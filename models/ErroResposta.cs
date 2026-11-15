namespace ExoApi.Models;

public class ErroResposta
{
    public ErroResposta(int status, string mensagem, List<string>? detalhes = null)
    {
        Status = status;
        Mensagem = mensagem;
        Detalhes = detalhes;
    }

    public int Status { get; set; }
    public string Mensagem { get; set; }
    public List<string>? Detalhes { get; set; }
}