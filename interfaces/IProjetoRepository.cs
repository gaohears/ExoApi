using ExoApi.Models;

namespace ExoApi.Interfaces;

public interface IProjetoRepository
{
    List<Projeto> Listar();
    Projeto? BuscarPorId(int id);
    Projeto Cadastrar(Projeto projeto);
    Projeto? Atualizar(int id, Projeto projeto);
    bool Deletar(int id);
}