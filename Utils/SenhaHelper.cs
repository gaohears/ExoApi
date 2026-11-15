using System.Security.Cryptography;
using System.Text;

namespace ExoApi.Utils;

public static class SenhaHelper
{
    // Transforma a senha em um hash SHA-256 (texto hexadecimal minúsculo)
    public static string Hash(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}