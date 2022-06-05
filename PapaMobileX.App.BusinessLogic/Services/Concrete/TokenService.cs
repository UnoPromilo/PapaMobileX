using System.IdentityModel.Tokens.Jwt;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public JwtSecurityToken? Token { get; private set; }

    public Task<string?> GetTokenAsync()
    {
        return Task.FromResult(Token?.ToString());
    }

    public bool IsTokenValid => ValidateToken();

    public TokenService()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
    }
    
    public void SaveToken(string token)
    {
        Token = _tokenHandler.ReadJwtToken(token);
    }

    public void ClearToken()
    {
        Token = null;
    }

    private bool ValidateToken()
    {
        return Token!.ValidFrom <= DateTime.UtcNow &&
               Token.ValidTo >= DateTime.UtcNow;
    }
}