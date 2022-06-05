using System.IdentityModel.Tokens.Jwt;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ITokenService
{
    JwtSecurityToken? SerializedToken { get; }
    
    string? Token { get; }
    
    bool IsTokenValid { get; }

    void SaveToken(string token);

    void ClearToken();
}