using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ILoginService
{
    public Task<Result<LoginError>> Login(LoginModel loginModel);
}