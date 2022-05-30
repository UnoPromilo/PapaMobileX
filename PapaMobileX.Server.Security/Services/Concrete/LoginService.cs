using System.Diagnostics.CodeAnalysis;
using PapaMobileX.Server.DataSource.Models;
using PapaMobileX.Server.DataSource.Repository;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Security.Services.Interfaces;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.Server.Security.Services.Concrete;

public class LoginService : ILoginService
{
    private readonly AccountsRepository _accountsRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public LoginService(IHashService hashService,
                        AccountsRepository accountsRepository,
                        ITokenService tokenService)
    {
        _hashService = hashService;
        _accountsRepository = accountsRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginError, LoginResultModel>> Login(LoginModel model)
    {
        Account? account = await _accountsRepository.GetAccountByUserName(model.UserName);
        if (VerifyAccount(account, model) == false)
            return LoginError.WrongCredentials();

        string token = _tokenService.GenerateNewToken(account);
        return new LoginResultModel
        {
            Token = token
        };
    }


    private bool VerifyAccount([NotNullWhen(true)] Account? account,
                               LoginModel model)
    {
        if (account is null)
            return false;

        string hash = _hashService.GetHash(model.Password, account.Salt);
        return hash == account.PasswordHash;
    }
}