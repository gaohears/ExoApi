using ExoApi.Models;

namespace ExoApi.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}