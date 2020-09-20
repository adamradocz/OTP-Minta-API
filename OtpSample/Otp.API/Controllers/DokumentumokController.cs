using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Logging;
using Otp.API.Models;
using Otp.API.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Otp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DokumentumokController : ControllerBase
    {
        private readonly ILogger<DokumentumokController> _logger;
        private readonly IDokumentumokService _dokumentumokRepository;

        public DokumentumokController(ILogger<DokumentumokController> logger, IDokumentumokService dokumentumokRepository)
        {
            _logger = logger;
            _dokumentumokRepository = dokumentumokRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetDokumentumok()
        {
            return Ok(_dokumentumokRepository.GetDokumentumok());
        }

        [HttpGet("{*fileName}", Name = "GetDokumentum")]
        [HttpHead("{*fileName}")]
        public async Task<ActionResult<string>> GetDokumentum(string fileName)
        {
            if (Request.Method.Equals("HEAD"))
            {
                var fileSize = _dokumentumokRepository.GetFileSize(fileName);
                if (fileSize == null)
                {
                    _logger.LogWarning($"Fájl nem létezik vagy érvénytelen fájlnév: {fileName}");
                    return NotFound("Fájl nem létezik vagy érvénytelen fájlnév.");
                }

                Response.ContentLength = fileSize;
                return Ok();
            }

            var file = await _dokumentumokRepository.GetDokumentum(fileName);

            if (file == null)
            {
                _logger.LogWarning($"Fájl nem létezik vagy érvénytelen fájlnév: {fileName}");
                return NotFound("Fájl nem létezik vagy érvénytelen fájlnév.");
            }

            return Ok(file);
        }

        [HttpPost("{*fileName}")]
        public async Task<ActionResult<string>> PostDokumentum(string fileName, [FromBody] string file)
        {
            var response = await _dokumentumokRepository.PostDokumentum(fileName, file);
            bool success = response.Item1;
            string message = response.Item2;

            if(!success)
            {
                _logger.LogWarning(message);
                return BadRequest(message);
            }

            return CreatedAtRoute("GetDokumentum", new { FileName = fileName }, message);
        }

        [HttpOptions]
        public IActionResult GetDokumentumokOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}
