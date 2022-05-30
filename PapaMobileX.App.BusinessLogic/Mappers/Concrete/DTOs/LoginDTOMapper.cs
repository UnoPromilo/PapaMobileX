using PapaMobileX.App.BusinessLogic.Mappers.Abstraction;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.DTOs.Requests;

namespace PapaMobileX.App.BusinessLogic.Mappers.Concrete.DTOs;

public class LoginDTOMapper : BaseMapper<LoginModel, LoginDTO>
{
    public override LoginDTO Map(LoginModel input)
    {
        return new LoginDTO
        {
            Password = input.Password,
            UserName = input.UserName
        };
    }
}