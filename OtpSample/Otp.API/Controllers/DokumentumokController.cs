using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Logging;
using Otp.API.Models;
using Otp.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Otp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DokumentumokController : ControllerBase
    {
        private readonly ILogger<DokumentumokController> _logger;
        private readonly IDokumentumokService _dokumentumokService;

        public DokumentumokController(ILogger<DokumentumokController> logger, IDokumentumokService dokumentumokService)
        {
            _logger = logger ?? throw new NullReferenceException();
            _dokumentumokService = dokumentumokService ?? throw new NullReferenceException();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetDokumentumok()
        {
            return Ok(_dokumentumokService.GetDokumentumok());
        }

        [HttpGet("{*fileName}", Name = "GetDokumentum")]
        public async Task<ActionResult<string>> GetDokumentum(string fileName, CancellationToken cancellationToken)
        {
            string file;
            try
            {
                file = await _dokumentumokService.GetDokumentum(fileName, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Letöltés megszakítva: {fileName}");
                file = "";
            }

            if (file == null)
            {
                _logger.LogWarning($"Fájl nem létezik vagy érvénytelen fájlnév: {fileName}");
                return NotFound("Fájl nem létezik vagy érvénytelen fájlnév.");
            }

            return Ok(file);
        }

        [HttpHead("{*fileName}")]
        public IActionResult GetDokumentum(string fileName)
        {
            var fileSize = _dokumentumokService.GetFileSize(fileName);
            if (fileSize == null)
            {
                _logger.LogWarning($"Fájl nem létezik vagy érvénytelen fájlnév: {fileName}");
                return NotFound("Fájl nem létezik vagy érvénytelen fájlnév.");
            }

            Response.ContentLength = fileSize;
            return Ok();
        }

        [HttpPost("{*fileName}")]
        public async Task<ActionResult<string>> PostDokumentum(string fileName, [FromBody] string file, CancellationToken cancellationToken)
        {
            (bool, string) response;
            try
            {
                response = await _dokumentumokService.PostDokumentum(fileName, file, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning($"Feltöltés megszakítva: {fileName}");
                return Ok("");
            }
            
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
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,HEAD");
            return Ok();
        }
    }
}
