using ExoApi.Exceptions;
using ExoApi.Interfaces;
using ExoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExoApi.Controllers;

/// <summary>
/// Gerenciamento de projetos da ExoApi.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjetosController : ControllerBase
{
    private static readonly string[] StatusValidos = { "Planejado", "EmAndamento", "Concluido", "Cancelado" };

    private readonly IProjetoRepository _repo;

    public ProjetosController(IProjetoRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Lista todos os projetos cadastrados.
    /// </summary>
    /// <response code="200">Lista de projetos (pode estar vazia).</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Projeto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status500InternalServerError)]
    public IActionResult Listar()
    {
        return Ok(_repo.Listar());
    }

    /// <summary>
    /// Busca um projeto pelo Id.
    /// </summary>
    /// <param name="id">Id do projeto.</param>
    /// <response code="200">Projeto encontrado.</response>
    /// <response code="404">Projeto não encontrado.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Projeto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status500InternalServerError)]
    public IActionResult BuscarPorId(int id)
    {
        var projeto = _repo.BuscarPorId(id);
        if (projeto == null)
        {
            throw new NaoEncontradoException($"Projeto com Id {id} não foi encontrado.");
        }

        return Ok(projeto);
    }

    /// <summary>
    /// Cadastra um novo projeto.
    /// </summary>
    /// <remarks>
    /// Status válidos: Planejado, EmAndamento, Concluido, Cancelado.
    /// </remarks>
    /// <param name="projeto">Dados do projeto. O Id é gerado pelo banco.</param>
    /// <response code="201">Projeto cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos ou status não permitido.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SucessoResposta), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status500InternalServerError)]
    public IActionResult Cadastrar(Projeto projeto)
    {
        ValidarStatus(projeto.Status);

        var criado = _repo.Cadastrar(projeto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id },
            new SucessoResposta(StatusCodes.Status201Created, "Projeto cadastrado com sucesso.", criado));
    }

    /// <summary>
    /// Atualiza todos os dados de um projeto existente.
    /// </summary>
    /// <param name="id">Id do projeto a atualizar.</param>
    /// <param name="projeto">Novos dados do projeto.</param>
    /// <response code="200">Projeto atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos ou status não permitido.</response>
    /// <response code="404">Projeto não encontrado.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SucessoResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Exclui um projeto.
    /// </summary>
    /// <param name="id">Id do projeto a excluir.</param>
    /// <response code="200">Projeto excluído com sucesso.</response>
    /// <response code="404">Projeto não encontrado.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(SucessoResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status500InternalServerError)]
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