using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ILoginService
{
    public Task<Result<LoginError>> Login(LoginModel loginModel);
}