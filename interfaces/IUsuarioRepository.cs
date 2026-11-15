using ExoApi.Models;

namespace ExoApi.Interfaces;

public interface IUsuarioRepository
{
    Usuario? Login(string email, string senhaHash);
}