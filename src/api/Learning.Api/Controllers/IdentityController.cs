using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Api.Controllers
{
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet("resource/authorized")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
