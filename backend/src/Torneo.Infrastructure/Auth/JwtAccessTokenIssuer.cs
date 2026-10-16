using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Torneo.Application.Abstractions;

namespace Torneo.Infrastructure.Auth;

internal sealed class JwtAccessTokenIssuer : IAccessTokenIssuer
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;

    public JwtAccessTokenIssuer(IConfiguration configuration)
    {
        _issuer = configuration["Auth:Issuer"] ?? "torneo";
        _audience = configuration["Auth:Audience"] ?? "torneo-frontend";
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:SigningKey"]!));
    }

    public string Issue(Guid operatorId, string username, string displayName)
    {
        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            [
                new Claim(JwtRegisteredClaimNames.Sub, operatorId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim("name", displayName)
            ],
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
