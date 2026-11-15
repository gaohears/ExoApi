using ExoApi.Exceptions;
using ExoApi.Interfaces;
using ExoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetosController : ControllerBase
{
    private static readonly string[] StatusValidos = { "Planejado", "EmAndamento", "Concluido", "Cancelado" };

    private readonly IProjetoRepository _repo;

    public ProjetosController(IProjetoRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_repo.Listar());
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var projeto = _repo.BuscarPorId(id);
        if (projeto == null)
        {
            throw new NaoEncontradoException($"Projeto com Id {id} não foi encontrado.");
        }

        return Ok(projeto);
    }

    [HttpPost]
    public IActionResult Cadastrar(Projeto projeto)
    {
        ValidarStatus(projeto.Status);

        var criado = _repo.Cadastrar(projeto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id },
            new SucessoResposta(StatusCodes.Status201Created, "Projeto cadastrado com sucesso.", criado));
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Projeto projeto)
    {
        ValidarStatus(projeto.Status);

        var atualizado = _repo.Atualizar(id, projeto);
        if (atualizado == null)
        {
            throw new NaoEncontradoException($"Projeto com Id {id} não foi encontrado.");
        }

        return Ok(new SucessoResposta(StatusCodes.Status200OK, "Projeto atualizado com sucesso.", atualizado));
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var deletou = _repo.Deletar(id);
        if (!deletou)
        {
            throw new NaoEncontradoException($"Projeto com Id {id} não foi encontrado.");
        }

        return Ok(new SucessoResposta(StatusCodes.Status200OK, "Projeto excluído com sucesso."));
    }

    private static void ValidarStatus(string status)
    {
        if (!StatusValidos.Contains(status, StringComparer.OrdinalIgnoreCase))
        {
            throw new RegraDeNegocioException(
                "Status inválido. Use: " + string.Join(", ", StatusValidos) + ".");
        }
    }
}