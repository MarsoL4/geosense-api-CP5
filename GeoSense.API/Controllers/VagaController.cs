using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.DTOs;
using GeoSense.API.Domain.Enums;

namespace GeoSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VagaController : ControllerBase
    {
        private readonly GeoSenseContext _context;

        public VagaController(GeoSenseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vaga>>> GetVagas()
        {
            return await _context.Vagas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vaga>> GetVaga(long id)
        {
            var vaga = await _context.Vagas.FindAsync(id);

            if (vaga == null)
                return NotFound();

            return vaga;
        }

        [HttpPost]
        public async Task<ActionResult<Vaga>> PostVaga(VagaDTO dto)
        {
            var novaVaga = new Vaga(dto.Numero, dto.PatioId);

            // caso queira permitir personalização opcional do tipo/status:
            novaVaga.GetType().GetProperty("Tipo")?.SetValue(novaVaga, (TipoVaga)dto.Tipo);
            novaVaga.GetType().GetProperty("Status")?.SetValue(novaVaga, (StatusVaga)dto.Status);

            _context.Vagas.Add(novaVaga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaga), new { id = novaVaga.Id }, novaVaga);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVaga(long id, VagaDTO dto)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
                return NotFound();

            // atualizações possíveis — via reflexão por causa dos setters privados
            vaga.GetType().GetProperty("Numero")?.SetValue(vaga, dto.Numero);
            vaga.GetType().GetProperty("Tipo")?.SetValue(vaga, (TipoVaga)dto.Tipo);
            vaga.GetType().GetProperty("Status")?.SetValue(vaga, (StatusVaga)dto.Status);
            vaga.GetType().GetProperty("PatioId")?.SetValue(vaga, dto.PatioId);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaga(long id)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
                return NotFound();

            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool VagaExists(long id)
        {
            return _context.Vagas.Any(v => v.Id == id);
        }
    }
}