using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Security.Services.Interfaces;

public interface ILoginService
{
    Task<Result<LoginError, LoginResultModel>> Login(LoginModel model);
}