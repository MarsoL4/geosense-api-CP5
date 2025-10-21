using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs.Vaga;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoSense.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/vaga")]
    [ApiController]
    [Produces("application/json")]
    public class VagaV2Controller : ControllerBase
    {
        private readonly VagaMongoRepository _repo;
        private readonly IMapper _mapper;

        public VagaV2Controller(VagaMongoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma lista paginada de vagas cadastradas (Mongo).
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de vagas (Mongo)", Description = "Retorna uma página com vagas armazenadas no MongoDB.")]
        [SwaggerResponse(200, "Lista paginada de vagas (Mongo)", typeof(IEnumerable<VagaDTO>))]
        public async Task<ActionResult> GetVagas([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var vagas = await _repo.ObterTodasAsync();
            var totalCount = vagas.Count;
            var paged = vagas.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<VagaDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Vagas", page, pageSize, totalCount);

            var result = new
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
        /// Retorna os dados de uma vaga por ID (Mongo).
        /// </summary>
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Buscar vaga por ID (Mongo)", Description = "Retorna os dados detalhados de uma vaga no MongoDB.")]
        [SwaggerResponse(200, "Vaga encontrada (Mongo)", typeof(VagaDTO))]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<ActionResult<VagaDTO>> GetVaga(long id)
        {
            var vaga = await _repo.ObterPorIdAsync(id);
            if (vaga == null) return NotFound(new { mensagem = "Vaga não encontrada." });

            var dto = _mapper.Map<VagaDTO>(vaga);
            return Ok(dto);
        }

        /// <summary>
        /// Cadastra uma nova vaga (Mongo).
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar vaga (Mongo)", Description = "Cadastra uma nova vaga no MongoDB.")]
        [SwaggerResponse(201, "Vaga criada com sucesso (Mongo)")]
        public async Task<ActionResult> PostVaga(VagaDTO dto)
        {
            var vagas = await _repo.ObterTodasAsync();
            var vagaExistente = vagas.Any(v => v.Numero == dto.Numero && v.PatioId == dto.PatioId);

            if (vagaExistente)
                return BadRequest(new { mensagem = "Já existe uma vaga com esse número neste pátio." });

            var novaVaga = new Vaga(dto.Numero, dto.PatioId)
            {
                Tipo = (TipoVaga)dto.Tipo,
                Status = (StatusVaga)dto.Status
            };

            await _repo.AdicionarAsync(novaVaga);

            var vagaCompleta = await _repo.ObterPorIdAsync(novaVaga.Id);
            var resultDto = _mapper.Map<VagaDTO>(vagaCompleta);

            return CreatedAtAction(nameof(GetVaga), new { id = novaVaga.Id, version = "2.0" }, new
            {
                mensagem = "Vaga cadastrada com sucesso (Mongo).",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de uma vaga existente (Mongo).
        /// </summary>
        [HttpPut("{id:long}")]
        [SwaggerOperation(Summary = "Atualizar vaga (Mongo)", Description = "Atualiza os dados de uma vaga armazenada no MongoDB.")]
        [SwaggerResponse(204, "Vaga atualizada com sucesso (Mongo)")]
        [SwaggerResponse(400, "Vaga duplicada no mesmo pátio")]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<IActionResult> PutVaga(long id, VagaDTO dto)
        {
            var vaga = await _repo.ObterPorIdAsync(id);
            if (vaga == null) return NotFound();

            var vagas = await _repo.ObterTodasAsync();
            var vagaExistente = vagas.Any(v => v.Numero == dto.Numero && v.PatioId == dto.PatioId && v.Id != id);

            if (vagaExistente)
                return BadRequest(new { mensagem = "Já existe uma vaga com esse número neste pátio." });

            vaga.Numero = dto.Numero;
            vaga.Tipo = (TipoVaga)dto.Tipo;
            vaga.Status = (StatusVaga)dto.Status;
            vaga.PatioId = dto.PatioId;

            await _repo.AtualizarAsync(vaga);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma vaga do sistema (Mongo).
        /// </summary>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Remover vaga (Mongo)", Description = "Remove a vaga informada pelo ID no MongoDB.")]
        [SwaggerResponse(204, "Vaga removida com sucesso (Mongo)")]
        [SwaggerResponse(404, "Vaga não encontrada")]
        public async Task<IActionResult> DeleteVaga(long id)
        {
            var vaga = await _repo.ObterPorIdAsync(id);
            if (vaga == null) return NotFound();

            await _repo.RemoverAsync(vaga);
            return NoContent();
        }
    }
}