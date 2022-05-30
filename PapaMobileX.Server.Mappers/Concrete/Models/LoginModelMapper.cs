using PapaMobileX.DTOs;
using PapaMobileX.DTOs.Requests;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Mappers.Abstractions;

namespace PapaMobileX.Server.Mappers.Concrete;

public class LoginModelMapper : BaseMapper<LoginDTO, LoginModel>
{
    public override LoginModel Map(LoginDTO dto)
    {
        return new LoginModel
        {
            Password = dto.Password,
            UserName = dto.UserName
        };
    }
}