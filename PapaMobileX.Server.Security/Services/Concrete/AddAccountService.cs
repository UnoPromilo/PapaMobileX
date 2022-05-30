using PapaMobileX.Server.BusinessLogic.Services.Interfaces;
using PapaMobileX.Server.DataSource.Models;
using PapaMobileX.Server.DataSource.Repository;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Security.Services.Interfaces;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Security.Services.Concrete;

public class AddAccountService : IAddAccountService
{
    private readonly AccountsRepository _accountsRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AddAccountService(AccountsRepository accountsRepository, IHashService hashService, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _accountsRepository = accountsRepository;
        _hashService = hashService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<AddAccountError>> AddAccount(NewAccountModel newAccountModel)
    {
        if (await CheckIfAccountExist(newAccountModel.UserName))
        {
            return AddAccountError.AccountAlreadyExist();
        }

        var salt = _hashService.GenerateSalt();
        var hashedPassword = _hashService.GetHash(newAccountModel.Password, salt);

        var account = new Account
        {
            Salt = salt,
            PasswordHash = hashedPassword,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = newAccountModel.UserName
        };
        
        _accountsRepository.AddAccount(account);
        await _unitOfWork.CommitAsync();
        return Result<AddAccountError>.Ok();
    }

    private async Task<bool> CheckIfAccountExist(string userName)
    {
        var existingAccount = await _accountsRepository.GetAccountByUserName(userName);
        return existingAccount is not null;
    }
}