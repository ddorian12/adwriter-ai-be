// AdWriter_AI/Controllers/AdWriterController.cs
using AdWriter_Application.Interfaces.Services;
using AdWriter_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdWriter_AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdWriterController : ControllerBase
    {
        private readonly IAdWriterService _service;

        public AdWriterController(IAdWriterService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] AdRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Invalid user.");

            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Description is required.");

            if (string.IsNullOrWhiteSpace(request.Language))
                request.Language = "ro"; // default la română dacă nu e specificată

            var result = await _service.GenerateAdContentAsync(request.Description, request.Language);
            return Ok(new { result });
        }
       
    }
}
