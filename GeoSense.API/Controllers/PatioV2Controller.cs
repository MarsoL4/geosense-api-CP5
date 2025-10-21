using AutoMapper;
using GeoSense.API.DTOs.Patio;
using GeoSense.API.Helpers;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/patio")]
    [ApiController]
    [Produces("application/json")]
    public class PatioV2Controller : ControllerBase
    {
        private readonly PatioService _service;
        private readonly IMapper _mapper;

        public PatioV2Controller(PatioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma lista paginada de pátios cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de pátios (v2)", Description = "Retorna pátios disponíveis para a versão v2.")]
        [SwaggerResponse(200, "Lista paginada de pátios", typeof(object))]
        public async Task<ActionResult> GetPatios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var patios = await _service.ObterTodasAsync();
            var totalCount = patios.Count;
            var paged = patios.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<PatioDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Patios", page, pageSize, totalCount);

            return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize, Links = links });
        }

        /// <summary>
        /// Retorna os dados detalhados de um pátio por ID.
        /// </summary>
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Buscar pátio por ID (v2)", Description = "Retorna dados detalhados de um pátio (v2).")]
        [SwaggerResponse(200, "Pátio encontrado", typeof(PatioDetalhesDTO))]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<ActionResult<PatioDetalhesDTO>> GetPatio(long id)
        {
            var patio = await _service.ObterPorIdAsync(id);

            if (patio == null)
                return NotFound(new { mensagem = "Pátio não encontrado." });

            var dto = new PatioDetalhesDTO
            {
                Id = patio.Id,
                Nome = patio.Nome,
                Vagas = patio.Vagas?.Select(v => _mapper.Map<GeoSense.API.DTOs.Vaga.VagaDTO>(v)).ToList() ?? new List<GeoSense.API.DTOs.Vaga.VagaDTO>()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cadastra um novo pátio.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar pátio (v2)", Description = "Cadastra novo pátio na versão v2.")]
        [SwaggerResponse(201, "Pátio criado com sucesso")]
        [SwaggerResponse(400, "Nome já existente")]
        [SwaggerRequestExample(typeof(PatioDTO), typeof(GeoSense.API.Examples.PatioDTOExample))]
        public async Task<ActionResult> PostPatio(PatioDTO dto)
        {
            var patios = await _service.ObterTodasAsync();
            var nomeExistente = patios.Any(p => p.Nome == dto.Nome);

            if (nomeExistente)
                return BadRequest(new { mensagem = "Já existe um pátio com esse nome." });

            var novoPatio = new Patio { Nome = dto.Nome };
            await _service.AdicionarAsync(novoPatio);

            var patioCompleto = await _service.ObterPorIdAsync(novoPatio.Id);
            var resultDto = _mapper.Map<PatioDTO>(patioCompleto);

            return CreatedAtAction(nameof(GetPatio), new { id = novoPatio.Id, version = "2.0" }, new
            {
                mensagem = "Pátio cadastrado com sucesso.",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        [HttpPut("{id:long}")]
        [SwaggerOperation(Summary = "Atualizar pátio (v2)", Description = "Atualiza um pátio existente na versão v2.")]
        [SwaggerResponse(204, "Pátio atualizado com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        [SwaggerRequestExample(typeof(PatioDTO), typeof(GeoSense.API.Examples.PatioDTOExample))]
        public async Task<IActionResult> PutPatio(long id, PatioDTO dto)
        {
            var patio = await _service.ObterPorIdAsync(id);
            if (patio == null) return NotFound();

            patio.Nome = dto.Nome;
            await _service.AtualizarAsync(patio);

            return NoContent();
        }

        /// <summary>
        /// Exclui um pátio do sistema.
        /// </summary>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Remover pátio (v2)", Description = "Remove pátio identificado pelo ID na versão v2.")]
        [SwaggerResponse(204, "Pátio removido com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<IActionResult> DeletePatio(long id)
        {
            var patio = await _service.ObterPorIdAsync(id);
            if (patio == null) return NotFound();

            await _service.RemoverAsync(patio);
            return NoContent();
        }
    }
}