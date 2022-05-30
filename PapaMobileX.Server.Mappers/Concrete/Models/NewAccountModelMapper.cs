using PapaMobileX.DTOs.Requests;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Mappers.Abstractions;

namespace PapaMobileX.Server.Mappers.Concrete.Models;

public class NewAccountModelMapper : BaseMapper<NewAccountDTO, NewAccountModel>
{
    public override NewAccountModel Map(NewAccountDTO input)
    {
        return new NewAccountModel
        {
            Password = input.Password,
            UserName = input.UserName
        };
    }
}