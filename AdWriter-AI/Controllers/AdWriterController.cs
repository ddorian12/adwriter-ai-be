// AdWriter_AI/Controllers/AdWriterController.cs
using AdWriter_Application.Interfaces;
using AdWriter_Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Description is required.");

            if (string.IsNullOrWhiteSpace(request.Language))
                request.Language = "ro"; // default la română dacă nu e specificată

            var result = await _service.GenerateAdContentAsync(request.Description, request.Language);
            return Ok(new { result });
        }


        [HttpPost("generate-2")]
        public async Task<ActionResult<AdContentResponse>> Generate2([FromBody] AdRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Description is required.");

            var result = await _service.GenerateAdContentAsync2(request.Description, request.Language);
            return Ok(result);
        }
    }
}
