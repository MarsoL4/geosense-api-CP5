// Substitua o arquivo atual por este (mantém o mesmo comportamento, só adiciona Swagger tags)
using GeoSense.API.Domain.Aggregates;
using GeoSense.API.Domain.Repositories;
using GeoSense.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoSense.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/vaga-aggregate")]
    [ApiController]
    public class VagaAggregateV2Controller : ControllerBase
    {
        private readonly IVagaAggregateRepository _repo;

        public VagaAggregateV2Controller(IVagaAggregateRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Aloca uma moto em uma vaga (regra do agregado).
        /// </summary>
        [HttpPost("{id:long}/alocar")]
        [SwaggerOperation(Summary = "Aloca uma moto em uma vaga (agregado)", Tags = new[] { "VagaAggregate (v2)" })]
        [SwaggerResponse(204, "Moto alocada com sucesso (agregado)")]
        [SwaggerResponse(400, "Solicitação inválida ou regra de negócio violada")]
        [SwaggerResponse(404, "Vaga ou Moto não encontrado")]
        public async Task<IActionResult> AlocarMoto(long id, [FromBody] AlocarMotoRequest body)
        {
            if (body == null || body.MotoId <= 0)
                return BadRequest(new { mensagem = "MotoId inválido." });

            var vagaAgg = await _repo.ObterPorIdAsync(id);
            if (vagaAgg == null)
                return NotFound(new { mensagem = "Vaga não encontrada." });

            try
            {
                vagaAgg.AlocarMoto(body.MotoId);
                await _repo.AtualizarAsync(vagaAgg);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Libera a vaga (regra do agregado).
        /// </summary>
        [HttpPost("{id:long}/liberar")]
        [SwaggerOperation(Summary = "Libera a vaga (agregado)", Tags = new[] { "VagaAggregate (v2)" })]
        [SwaggerResponse(204, "Vaga liberada com sucesso (agregado)")]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<IActionResult> LiberarVaga(long id)
        {
            var vagaAgg = await _repo.ObterPorIdAsync(id);
            if (vagaAgg == null)
                return NotFound(new { mensagem = "Vaga não encontrada." });

            vagaAgg.LiberarVaga();
            await _repo.AtualizarAsync(vagaAgg);
            return NoContent();
        }

        /// <summary>
        /// Request para alocar moto
        /// </summary>
        public class AlocarMotoRequest
        {
            public long MotoId { get; set; }
        }
    }
}