using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Auth.Login;

namespace Torneo.Api.Auth;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController(LoginHandler login) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginSession), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Login([FromBody] LoginDto request, CancellationToken cancellationToken)
    {
        var result = await login.Handle(new LoginCommand(request.Username ?? "", request.Password ?? ""), cancellationToken);
        return HttpResults.Ok(result);
    }
}
