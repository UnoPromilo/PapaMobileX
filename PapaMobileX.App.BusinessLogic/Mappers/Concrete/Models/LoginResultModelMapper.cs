using PapaMobileX.App.BusinessLogic.Mappers.Abstraction;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.DTOs.Responses;

namespace PapaMobileX.App.BusinessLogic.Mappers.Concrete.Models;

public class LoginResultModelMapper : BaseMapper<LoginResultDTO, LoginResultModel>
{
    public override LoginResultModel Map(LoginResultDTO input)
    {
        return new LoginResultModel
        {
            Token = input.Token
        };
    }
}