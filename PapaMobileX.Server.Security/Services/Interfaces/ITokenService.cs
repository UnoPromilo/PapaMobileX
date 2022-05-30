using PapaMobileX.Server.DataSource.Models;

namespace PapaMobileX.Server.Security.Services.Interfaces;

public interface ITokenService
{
    public string GenerateNewToken(Account account);
}