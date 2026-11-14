using ExoApi.Contexts;
using ExoApi.Interfaces;
using ExoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoApi.Repositories;

public class ProjetoRepository : IProjetoRepository
{
    private readonly ExoApiContext _context;

    public ProjetoRepository(ExoApiContext context)
    {
        _context = context;
    }

    public List<Projeto> Listar()
    {
        return _context.Projetos.AsNoTracking().ToList();
    }

    public Projeto? BuscarPorId(int id)
    {
        return _context.Projetos.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public Projeto Cadastrar(Projeto projeto)
    {
        projeto.Id = 0; // o banco gera o Id sozinho
        _context.Projetos.Add(projeto);
        _context.SaveChanges();
        return projeto;
    }

    public Projeto? Atualizar(int id, Projeto projeto)
    {
        var existente = _context.Projetos.Find(id);
        if (existente == null)
        {
            return null;
        }

        existente.Nome = projeto.Nome;
        existente.Descricao = projeto.Descricao;
        existente.DataInicio = projeto.DataInicio;
        existente.Status = projeto.Status;

        _context.SaveChanges();
        return existente;
    }

    public bool Deletar(int id)
    {
        var existente = _context.Projetos.Find(id);
        if (existente == null)
        {
            return false;
        }

        _context.Projetos.Remove(existente);
        _context.SaveChanges();
        return true;
    }
}