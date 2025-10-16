using Microsoft.AspNetCore.Mvc;
using GeoSense.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoSense.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DashboardController(DashboardService service) : ControllerBase
    {
        private readonly DashboardService _service = service;

        /// <summary>
        /// Retorna dados agregados para o dashboard: totais de motos, vagas e problemas.
        /// </summary>
        /// <remarks>
        /// Retorna informações resumidas sobre o sistema, incluindo total de motos, motos com problema, vagas livres e ocupadas.
        /// </remarks>
        /// <response code="200">Dados agregados para o dashboard</response>
        [HttpGet]
        [SwaggerResponse(200, "Dados agregados para o dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var resultado = await _service.ObterDashboardDataAsync();
            return Ok(resultado);
        }
    }
}