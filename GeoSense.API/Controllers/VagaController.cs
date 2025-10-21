using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs;
using GeoSense.API.DTOs.Vaga;
using GeoSense.API.Helpers;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VagaController(VagaService service, IMapper mapper) : ControllerBase
    {
        private readonly VagaService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de vagas cadastradas.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de vagas", Description = "Retorna uma página com vagas cadastradas.")]
        [SwaggerResponse(200, "Lista paginada de vagas cadastradas", typeof(PagedHateoasDTO<VagaDTO>))]
        public async Task<ActionResult<PagedHateoasDTO<VagaDTO>>> GetVagas([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var vagas = await _service.ObterTodasAsync();
            var totalCount = vagas.Count;
            var paged = vagas.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<VagaDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Vagas", page, pageSize, totalCount);

            var result = new PagedHateoasDTO<VagaDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Links = links
            };

            return Ok(result);
        }

        /// <summary>
        /// Retorna os dados de uma vaga por ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar vaga por ID", Description = "Retorna os detalhes de uma vaga específica a partir do seu identificador.")]
        [SwaggerResponse(200, "Vaga encontrada", typeof(VagaDTO))]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<ActionResult<VagaDTO>> GetVaga(long id)
        {
            var vaga = await _service.ObterPorIdAsync(id);

            if (vaga == null)
                return NotFound(new { mensagem = "Vaga não encontrada." });

            var dto = _mapper.Map<VagaDTO>(vaga);
            return Ok(dto);
        }

        /// <summary>
        /// Cadastra uma nova vaga.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar vaga", Description = "Cadastra uma nova vaga e retorna o recurso criado.")]
        [SwaggerRequestExample(typeof(VagaDTO), typeof(GeoSense.API.Examples.VagaDTOExample))]
        [SwaggerResponse(201, "Vaga criada com sucesso", typeof(object))]
        [SwaggerResponse(400, "Vaga duplicada no mesmo pátio")]
        public async Task<ActionResult<VagaDTO>> PostVaga(VagaDTO dto)
        {
            var vagas = await _service.ObterTodasAsync();
            var vagaExistente = vagas.Any(v => v.Numero == dto.Numero && v.PatioId == dto.PatioId);

            if (vagaExistente)
                return BadRequest(new { mensagem = "Já existe uma vaga com esse número neste pátio." });

            // Use AutoMapper to create a Vaga instance
            var novaVaga = _mapper.Map<Vaga>(dto);

            await _service.AdicionarAsync(novaVaga);

            var vagaCompleta = await _service.ObterPorIdAsync(novaVaga.Id);
            var resultDto = _mapper.Map<VagaDTO>(vagaCompleta);

            return CreatedAtAction(nameof(GetVaga), new { id = novaVaga.Id }, new
            {
                mensagem = "Vaga cadastrada com sucesso.",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de uma vaga existente.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar vaga", Description = "Atualiza os dados de uma vaga existente.")]
        [SwaggerRequestExample(typeof(VagaDTO), typeof(GeoSense.API.Examples.VagaDTOExample))]
        [SwaggerResponse(204, "Vaga atualizada com sucesso (No Content)")]
        [SwaggerResponse(400, "Vaga duplicada no mesmo pátio")]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<IActionResult> PutVaga(long id, VagaDTO dto)
        {
            var vaga = await _service.ObterPorIdAsync(id);
            if (vaga == null)
                return NotFound();

            var vagas = await _service.ObterTodasAsync();
            var vagaExistente = vagas.Any(v => v.Numero == dto.Numero && v.PatioId == dto.PatioId && v.Id != id);

            if (vagaExistente)
                return BadRequest(new { mensagem = "Já existe uma vaga com esse número neste pátio." });

            // Atribuições diretas (evita reflection)
            vaga.Numero = dto.Numero;
            vaga.Tipo = (TipoVaga)dto.Tipo;
            vaga.Status = (StatusVaga)dto.Status;
            vaga.PatioId = dto.PatioId;

            await _service.AtualizarAsync(vaga);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma vaga do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover vaga", Description = "Remove a vaga informada pelo ID.")]
        [SwaggerResponse(204, "Vaga removida com sucesso (No Content)")]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<IActionResult> DeleteVaga(long id)
        {
            var vaga = await _service.ObterPorIdAsync(id);
            if (vaga == null)
                return NotFound();

            await _service.RemoverAsync(vaga);
            return NoContent();
        }
    }
}