using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.Server.Security.Services.Interfaces;

public interface IAddAccountService
{
    Task<Result<AddAccountError>> AddAccount(NewAccountModel newAccountModel);
}