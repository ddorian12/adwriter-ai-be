using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdWriter_AI.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("AdWriter AI API is running.");
        }
    }
}
