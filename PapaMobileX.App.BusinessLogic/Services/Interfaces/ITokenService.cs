using System.IdentityModel.Tokens.Jwt;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ITokenService
{
    JwtSecurityToken? Token { get; }
    bool IsTokenValid { get; }

    void SaveToken(string token);

    void ClearToken();
}