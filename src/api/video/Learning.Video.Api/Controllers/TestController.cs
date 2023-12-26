using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Video.Api.Controllers;

[ApiController]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        return Ok(User.Claims.ToList());
    }
}
