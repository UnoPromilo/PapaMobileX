using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PapaMobileX.Server.Controllers;

[Authorize]
[Route("/tokenCheck")]
public class TokenCheckController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
    {
        return Ok();
    }
}