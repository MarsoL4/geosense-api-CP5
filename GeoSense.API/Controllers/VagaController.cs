using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs;
using GeoSense.API.DTOs.Vaga;
using GeoSense.API.Helpers;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VagaController(VagaService service, IMapper mapper) : ControllerBase
    {
        private readonly VagaService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de vagas cadastradas.
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de vagas, podendo utilizar paginação via parâmetros <b>page</b> e <b>pageSize</b>.
        /// </remarks>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <response code="200">Lista paginada de vagas</response>
        [HttpGet]
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
        /// <remarks>
        /// Retorna os detalhes de uma vaga específica a partir do seu identificador.
        /// </remarks>
        /// <param name="id">Identificador único da vaga</param>
        /// <response code="200">Vaga encontrada</response>
        /// <response code="404">Vaga não encontrada</response>
        [HttpGet("{id}")]
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
        /// <remarks>
        /// Cadastra uma nova vaga no sistema. O corpo da requisição deve conter o modelo <see cref="VagaDTO"/>.
        /// </remarks>
        /// <param name="dto">Dados da nova vaga</param>
        /// <response code="201">Vaga criada com sucesso</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(VagaDTO), typeof(GeoSense.API.Examples.VagaDTOExample))]
        [SwaggerResponse(201, "Vaga criada com sucesso", typeof(object))]
        public async Task<ActionResult<VagaDTO>> PostVaga(VagaDTO dto)
        {
            var vagas = await _service.ObterTodasAsync();
            var vagaExistente = vagas.Any(v => v.Numero == dto.Numero && v.PatioId == dto.PatioId);

            if (vagaExistente)
                return BadRequest(new { mensagem = "Já existe uma vaga com esse número neste pátio." });

            var novaVaga = new GeoSense.API.Infrastructure.Persistence.Vaga(dto.Numero, dto.PatioId);
            novaVaga.GetType().GetProperty("Tipo")?.SetValue(novaVaga, (TipoVaga)dto.Tipo);
            novaVaga.GetType().GetProperty("Status")?.SetValue(novaVaga, (StatusVaga)dto.Status);

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
        /// <remarks>
        /// Atualiza os dados da vaga informada pelo ID. O corpo da requisição deve conter o modelo <see cref="VagaDTO"/>.
        /// </remarks>
        /// <param name="id">Identificador único da vaga</param>
        /// <param name="dto">Dados da vaga a serem atualizados</param>
        /// <response code="204">Vaga atualizada com sucesso (No Content)</response>
        /// <response code="400">Vaga duplicada no mesmo pátio</response>
        /// <response code="404">Vaga não encontrada</response>
        [HttpPut("{id}")]
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

            vaga.GetType().GetProperty("Numero")?.SetValue(vaga, dto.Numero);
            vaga.GetType().GetProperty("Tipo")?.SetValue(vaga, (TipoVaga)dto.Tipo);
            vaga.GetType().GetProperty("Status")?.SetValue(vaga, (StatusVaga)dto.Status);
            vaga.GetType().GetProperty("PatioId")?.SetValue(vaga, dto.PatioId);

            await _service.AtualizarAsync(vaga);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma vaga do sistema.
        /// </summary>
        /// <remarks>
        /// Remove a vaga informada pelo ID.
        /// </remarks>
        /// <param name="id">Identificador único da vaga</param>
        /// <response code="204">Vaga removida com sucesso (No Content)</response>
        /// <response code="404">Vaga não encontrada</response>
        [HttpDelete("{id}")]
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