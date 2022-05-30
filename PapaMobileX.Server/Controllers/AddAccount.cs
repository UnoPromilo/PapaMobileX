using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PapaMobileX.DTOs.Requests;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Mappers.Abstractions;
using PapaMobileX.Server.Security.Services.Interfaces;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Controllers;

[ApiController]
[Route("add-account")]
public class AddAccount : ControllerBase
{
    private readonly IMapper<NewAccountDTO, NewAccountModel> _mapper;
    private readonly IAddAccountService _addAccountService;

    public AddAccount(IMapper<NewAccountDTO, NewAccountModel> mapper, IAddAccountService addAccountService)
    {
        _mapper = mapper;
        _addAccountService = addAccountService;
    }

    [HttpPost(Name = "Add account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(AddAccountError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAccountAsync([FromBody] NewAccountDTO dto)
    {
        var model = _mapper.Map(dto);
        var result = await _addAccountService.AddAccount(model);
        if (result.IsSuccess)
        {
            return Ok();
        }
        
        return new ObjectResult(result.Error)
        {
            StatusCode = (int)result.Error!.StatusCode
        };
    }
    
}