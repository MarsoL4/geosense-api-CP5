using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GeoSense.API.Application.Services;

namespace GeoSense.API.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DashboardController(DashboardService service) : ControllerBase
    {
        private readonly DashboardService _service = service;

        /// <summary>
        /// Retorna dados agregados para o dashboard: totais de motos, vagas e problemas.
        /// </summary>
        /// <remarks>
        /// Retorna informações resumidas sobre o sistema, incluindo total de motos, motos com problema, vagas livres e ocupadas.
        /// </remarks>
        [HttpGet]
        [SwaggerOperation(Summary = "Dados agregados do dashboard", Description = "Retorna métricas agregadas úteis para o dashboard administrativo.")]
        [SwaggerResponse(200, "Dados agregados para o dashboard", typeof(object))]
        public async Task<IActionResult> GetDashboardData()
        {
            var resultado = await _service.ObterDashboardDataAsync();
            return Ok(resultado);
        }
    }
}