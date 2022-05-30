using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PapaMobileX.Server.DataSource.Models;
using PapaMobileX.Server.Security.Services.Interfaces;
using Claim = System.Security.Claims.Claim;

namespace PapaMobileX.Server.Security.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateNewToken(Account account)
    {
        var claims = new List<Claim>
        {
            new(Constants.AccountGuidKey, account.AccountGuid.ToString()),
            new(Constants.UsernameKey, account.UserName),
        };
        
        DateTime expiresAt = DateTime.Today.AddDays(1);

        return CreateToken(claims, expiresAt);
    }
    
    private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
    {
        var secret = _configuration.GetValue<string>(Constants.JWTSecretConfigKey);
        byte[] key = Encoding.ASCII.GetBytes(secret);

        var jwt = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms
                    .HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}