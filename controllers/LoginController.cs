using ExoApi.Exceptions;
using ExoApi.Interfaces;
using ExoApi.Models;
using ExoApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ExoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoginController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ITokenService _tokenService;

    public LoginController(IUsuarioRepository usuarioRepo, ITokenService tokenService)
    {
        _usuarioRepo = usuarioRepo;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Autentica o usuário e devolve um token JWT.
    /// </summary>
    /// <param name="login">E-mail e senha do usuário.</param>
    /// <response code="200">Login realizado. O token deve ser enviado no cabeçalho Authorization.</response>
    /// <response code="400">E-mail ou senha não informados.</response>
    /// <response code="401">E-mail ou senha inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(LoginResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status401Unauthorized)]
    public IActionResult Login(LoginRequest login)
    {
        var usuario = _usuarioRepo.Login(login.Email, SenhaHelper.Hash(login.Senha));
        if (usuario == null)
        {
            throw new CredenciaisInvalidasException("E-mail ou senha inválidos.");
        }

        return Ok(new LoginResposta { Token = _tokenService.GerarToken(usuario) });
    }
}