using Microsoft.AspNetCore.Mvc;
using PapaMobileX.DTOs;
using PapaMobileX.DTOs.Requests;
using PapaMobileX.DTOs.Responses;
using PapaMobileX.Server.Entities.Models;
using PapaMobileX.Server.Mappers.Abstractions;
using PapaMobileX.Server.Mappers.Concrete;
using PapaMobileX.Server.Security.Services.Interfaces;
using PapaMobileX.Server.Shared.Errors;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.Server.Controllers;

[Route("login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IMapper<LoginDTO, LoginModel> _loginModelMapper;
    private readonly IMapper<LoginResultModel, LoginResultDTO> _loginResultMapper;
    private readonly ILoginService _loginService;

    public LoginController(
        IMapper<LoginDTO, LoginModel> loginModelMapper,
        IMapper<LoginResultModel, LoginResultDTO> loginResultMapper,
        ILoginService loginService)
    {
        _loginModelMapper = loginModelMapper;
        _loginResultMapper = loginResultMapper;
        _loginService = loginService;
    }

    [HttpPost(Name = "Login")]
    [ProducesResponseType(typeof(LoginResultDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(LoginError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginAsync(LoginDTO dto)
    {
        var model = _loginModelMapper.Map(dto);
        var result = await _loginService.Login(model);
        if (result.IsSuccess)
        {
            return Ok(_loginResultMapper.Map(result.Data));
        }
        
        return new ObjectResult(result.Error)
        {
            StatusCode = (int)result.Error!.StatusCode
        };
    }
}