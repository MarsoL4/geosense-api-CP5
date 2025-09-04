using Microsoft.AspNetCore.Mvc;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Application.Services;

namespace GeoSense.API.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly VagaService _vagaService;

        public VagaController(VagaService vagaService)
        {
            _vagaService = vagaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VagaDTO>>> GetVagas()
        {
            var vagas = await _vagaService.ObterTodasAsync();
            return Ok(vagas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VagaDTO>> GetVaga(long id)
        {
            var vaga = await _vagaService.ObterPorIdAsync(id);
            if (vaga == null)
                return NotFound();

            return Ok(vaga);
        }

        [HttpPost]
        public async Task<ActionResult> PostVaga(VagaDTO dto)
        {
            var id = await _vagaService.CriarAsync(dto);
            if (id == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetVaga), new { id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVaga(long id, VagaDTO dto)
        {
            var updated = await _vagaService.AtualizarAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaga(long id)
        {
            var deleted = await _vagaService.RemoverAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}