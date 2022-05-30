using PapaMobileX.DTOs.Responses;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Mappers.Abstractions;

namespace PapaMobileX.Server.Mappers.Concrete.DTOs;

public class LoginResultDTOMapper : BaseMapper<LoginResultModel, LoginResultDTO>
{
    public override LoginResultDTO Map(LoginResultModel input)
    {
        return new LoginResultDTO
        {
            Token = input.Token
        };
    }
}