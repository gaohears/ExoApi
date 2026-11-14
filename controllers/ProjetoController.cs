using ExoApi.Interfaces;
using ExoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetosController : ControllerBase
{
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
            return NotFound();
        }

        return Ok(projeto);
    }

    [HttpPost]
    public IActionResult Cadastrar(Projeto projeto)
    {
        var criado = _repo.Cadastrar(projeto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Projeto projeto)
    {
        var atualizado = _repo.Atualizar(id, projeto);
        if (atualizado == null)
        {
            return NotFound();
        }

        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var deletou = _repo.Deletar(id);
        if (!deletou)
        {
            return NotFound();
        }

        return NoContent();
    }
}