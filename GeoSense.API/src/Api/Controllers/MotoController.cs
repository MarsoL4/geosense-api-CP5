using Microsoft.AspNetCore.Mvc;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Application.Services;

namespace GeoSense.API.src.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _motoService;

        public MotoController(MotoService motoService)
        {
            _motoService = motoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotoListagemDTO>>> GetMotos()
        {
            var motos = await _motoService.ObterTodasAsync();
            return Ok(motos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotoDetalhesDTO>> GetMoto(long id)
        {
            var moto = await _motoService.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound();

            return Ok(moto);
        }

        [HttpPost]
        public async Task<ActionResult<MotoDetalhesDTO>> PostMoto(MotoDTO dto)
        {
            var moto = await _motoService.CriarAsync(dto);
            if (moto == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoto(long id, MotoDTO dto)
        {
            var updated = await _motoService.AtualizarAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(long id)
        {
            var deleted = await _motoService.RemoverAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}