namespace PapaMobileX.Server.Security.Services.Interfaces;

public interface IHashService
{
    string GetHash(string password, string salt);

    string GenerateSalt();
}