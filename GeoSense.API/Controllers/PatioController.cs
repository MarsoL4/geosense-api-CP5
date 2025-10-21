using AutoMapper;
using GeoSense.API.DTOs;
using GeoSense.API.DTOs.Patio;
using GeoSense.API.DTOs.Vaga;
using GeoSense.API.Helpers;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Linq;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PatioController(PatioService service, IMapper mapper) : ControllerBase
    {
        private readonly PatioService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de pátios cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de pátios", Description = "Retorna uma página com pátios cadastrados.")]
        [SwaggerResponse(200, "Lista paginada de pátios cadastrados", typeof(PagedHateoasDTO<PatioDTO>))]
        public async Task<ActionResult<PagedHateoasDTO<PatioDTO>>> GetPatios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var patios = await _service.ObterTodasAsync();
            var totalCount = patios.Count;
            var paged = patios.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<PatioDTO>>(paged);

            // padronizado
            var links = HateoasHelper.GetPagedLinks(Url, "patio", page, pageSize, totalCount);

            var result = new PagedHateoasDTO<PatioDTO>
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
        /// Retorna os dados detalhados de um pátio por ID.
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar pátio por ID", Description = "Retorna os detalhes de um pátio específico, incluindo suas vagas.")]
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
        /// Cadastra um novo pátio no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar pátio", Description = "Cadastra um novo pátio.")]
        [SwaggerRequestExample(typeof(PatioDTO), typeof(GeoSense.API.Examples.PatioDTOExample))]
        [SwaggerResponse(201, "Pátio criado com sucesso", typeof(object))]
        [SwaggerResponse(400, "Nome já existente")]
        public async Task<ActionResult<PatioDTO>> PostPatio(PatioDTO _dto)
        {
            var patios = await _service.ObterTodasAsync();
            var nomeExistente = patios.Any(p => p.Nome == _dto.Nome);

            if (nomeExistente)
                return BadRequest(new { mensagem = "Já existe um pátio com esse nome." });

            // Use AutoMapper to create entity (avoid direct dependency on persistence types in controller)
            var novoPatio = _mapper.Map<Patio>(_dto);
            await _service.AdicionarAsync(novoPatio);

            var patioCompleto = await _service.ObterPorIdAsync(novoPatio.Id);
            var resultDto = _mapper.Map<PatioDTO>(patioCompleto);

            return CreatedAtAction(nameof(GetPatio), new { id = novoPatio.Id }, new
            {
                mensagem = "Pátio cadastrado com sucesso.",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar pátio", Description = "Atualiza o nome de um pátio existente.")]
        [SwaggerRequestExample(typeof(PatioDTO), typeof(GeoSense.API.Examples.PatioDTOExample))]
        [SwaggerResponse(204, "Pátio atualizado com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<IActionResult> PutPatio(long id, PatioDTO _dto)
        {
            var patio = await _service.ObterPorIdAsync(id);
            if (patio == null) return NotFound();

            patio.Nome = _dto.Nome;
            await _service.AtualizarAsync(patio);

            return NoContent();
        }

        /// <summary>
        /// Exclui um pátio do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover pátio", Description = "Remove o pátio informado pelo ID.")]
        [SwaggerResponse(204, "Pátio removido com sucesso (No Content)")]
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