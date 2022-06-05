using System.IdentityModel.Tokens.Jwt;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ITokenService
{
    JwtSecurityToken? Token { get; }

    /// <summary>
    /// Use only when you need task
    /// </summary>
    Task<string?> GetTokenAsync();
    bool IsTokenValid { get; }

    void SaveToken(string token);

    void ClearToken();
}