using System.IdentityModel.Tokens.Jwt;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public JwtSecurityToken? SerializedToken => Token == null ? null : _tokenHandler.ReadJwtToken(Token);

    public string? Token { get; private set; }

    public bool IsTokenValid => ValidateToken();

    public void SaveToken(string token)
    {
        Token = token;
    }

    public void ClearToken()
    {
        Token = null;
    }

    private bool ValidateToken()
    {
        if (SerializedToken is null)
            return false;

        return (SerializedToken!.ValidFrom <= DateTime.UtcNow) &&
               (SerializedToken.ValidTo >= DateTime.UtcNow);
    }
}