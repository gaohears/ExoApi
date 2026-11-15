using ExoApi.Contexts;
using ExoApi.Interfaces;
using ExoApi.Models;

namespace ExoApi.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ExoApiContext _context;

    public UsuarioRepository(ExoApiContext context)
    {
        _context = context;
    }

    public Usuario? Login(string email, string senhaHash)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senhaHash);
    }
}