using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Domain.Enums;

namespace GeoSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly GeoSenseContext _context;

        public DashboardController(GeoSenseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            // Total de motos cadastradas
            var totalMotos = await _context.Motos.CountAsync();

            // Total de motos com problema identificado
            var motosComProblema = await _context.Motos
                .CountAsync(m => !string.IsNullOrEmpty(m.ProblemaIdentificado));

            // Vagas por status
            var vagasLivres = await _context.Vagas
                .CountAsync(v => v.Status == StatusVaga.LIVRE);

            var vagasOcupadas = await _context.Vagas
                .CountAsync(v => v.Status == StatusVaga.OCUPADA);

            var totalVagas = vagasLivres + vagasOcupadas;

            var resultado = new
            {
                TotalMotos = totalMotos,
                MotosComProblema = motosComProblema,
                VagasLivres = vagasLivres,
                VagasOcupadas = vagasOcupadas,
                TotalVagas = totalVagas
            };

            return Ok(resultado);
        }
    }
}