using AutoMapper;
using GeoSense.API.DTOs.Moto;
using GeoSense.API.Helpers;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoSense.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MotoV2Controller : ControllerBase
    {
        private readonly MotoMongoRepository _repo;
        private readonly IMapper _mapper;

        public MotoV2Controller(MotoMongoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma lista paginada de motos cadastradas (Mongo).
        /// </summary>
        [HttpGet]
        [SwaggerResponse(200, "Lista paginada de motos (Mongo)", typeof(object))]
        public async Task<ActionResult> GetMotos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var motos = await _repo.ObterTodasAsync();
            var totalCount = motos.Count;
            var paged = motos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<MotoDetalhesDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Motos", page, pageSize, totalCount);

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
        /// Retorna os dados de uma moto por ID (Mongo).
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Moto encontrada (Mongo)", typeof(MotoDetalhesDTO))]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<ActionResult<MotoDetalhesDTO>> GetMoto(long id)
        {
            var moto = await _repo.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound(new { mensagem = "Moto não encontrada." });

            var dto = _mapper.Map<MotoDetalhesDTO>(moto);
            return Ok(dto);
        }

        /// <summary>
        /// Cadastra uma nova moto (Mongo).
        /// </summary>
        [HttpPost]
        [SwaggerResponse(201, "Moto criada com sucesso (Mongo)")]
        [SwaggerResponse(400, "Alguma restrição de negócio foi violada")]
        public async Task<ActionResult> PostMoto(MotoDTO dto)
        {
            var motos = await _repo.ObterTodasAsync();

            var vagaOcupada = motos.Any(m => m.VagaId == dto.VagaId);
            if (vagaOcupada)
                return BadRequest(new { mensagem = "Esta vaga já está ocupada por outra moto." });

            var placaExiste = motos.Any(m => m.Placa == dto.Placa);
            if (placaExiste)
                return BadRequest(new { mensagem = "Já existe uma moto com essa placa." });

            var chassiExiste = motos.Any(m => m.Chassi == dto.Chassi);
            if (chassiExiste)
                return BadRequest(new { mensagem = "Já existe uma moto com esse chassi." });

            var novaMoto = new Moto
            {
                Modelo = dto.Modelo,
                Placa = dto.Placa,
                Chassi = dto.Chassi,
                ProblemaIdentificado = dto.ProblemaIdentificado,
                VagaId = dto.VagaId
            };

            await _repo.AdicionarAsync(novaMoto);

            var motoCompleta = await _repo.ObterPorIdAsync(novaMoto.Id);
            var resultDto = _mapper.Map<MotoDetalhesDTO>(motoCompleta);

            return CreatedAtAction(nameof(GetMoto), new { id = novaMoto.Id, version = "2.0" }, new
            {
                mensagem = "Moto cadastrada com sucesso (Mongo).",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente (Mongo).
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerResponse(204, "Moto atualizada com sucesso (Mongo)")]
        [SwaggerResponse(400, "Restrição de negócio violada")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> PutMoto(long id, MotoDTO dto)
        {
            var moto = await _repo.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound();

            var motos = await _repo.ObterTodasAsync();

            var vagaOcupada = motos.Any(m => m.VagaId == dto.VagaId && m.Id != id);
            if (vagaOcupada)
                return BadRequest(new { mensagem = "Esta vaga já está ocupada por outra moto." });

            var placaExiste = motos.Any(m => m.Placa == dto.Placa && m.Id != id);
            if (placaExiste)
                return BadRequest(new { mensagem = "Já existe uma moto com essa placa." });

            var chassiExiste = motos.Any(m => m.Chassi == dto.Chassi && m.Id != id);
            if (chassiExiste)
                return BadRequest(new { mensagem = "Já existe uma moto com esse chassi." });

            moto.Modelo = dto.Modelo;
            moto.Placa = dto.Placa;
            moto.Chassi = dto.Chassi;
            moto.ProblemaIdentificado = dto.ProblemaIdentificado;
            moto.VagaId = dto.VagaId;

            await _repo.AtualizarAsync(moto);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma moto do sistema (Mongo).
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Moto removida com sucesso (Mongo)")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> DeleteMoto(long id)
        {
            var moto = await _repo.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound();

            await _repo.RemoverAsync(moto);
            return NoContent();
        }
    }
}