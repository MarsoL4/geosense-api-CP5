using AutoMapper;
using GeoSense.API.DTOs;
using GeoSense.API.DTOs.Moto;
using GeoSense.API.Helpers;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MotoController(MotoService service, IMapper mapper) : ControllerBase
    {
        private readonly MotoService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de motos cadastradas.
        /// </summary>
        /// <remarks>
        /// Retorna uma lista de motos, podendo utilizar paginação via parâmetros <b>page</b> e <b>pageSize</b>.
        /// </remarks>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <response code="200">Lista paginada de motos</response>
        [HttpGet]
        [SwaggerResponse(200, "Lista paginada de motos cadastradas", typeof(PagedHateoasDTO<MotoDetalhesDTO>))]
        public async Task<ActionResult<PagedHateoasDTO<MotoDetalhesDTO>>> GetMotos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var motos = await _service.ObterTodasAsync();
            var totalCount = motos.Count;
            var paged = motos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<MotoDetalhesDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Motos", page, pageSize, totalCount);

            var result = new PagedHateoasDTO<MotoDetalhesDTO>
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
        /// Retorna os dados de uma moto por ID.
        /// </summary>
        /// <remarks>
        /// Retorna os detalhes de uma moto específica a partir do seu identificador.
        /// </remarks>
        /// <param name="id">Identificador único da moto</param>
        /// <response code="200">Moto encontrada</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Moto encontrada", typeof(MotoDetalhesDTO))]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<ActionResult<MotoDetalhesDTO>> GetMoto(long id)
        {
            var moto = await _service.ObterPorIdAsync(id);

            if (moto == null)
            {
                return NotFound(new { mensagem = "Moto não encontrada." });
            }

            var dto = _mapper.Map<MotoDetalhesDTO>(moto);
            return Ok(dto);
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente.
        /// </summary>
        /// <remarks>
        /// Atualiza os dados da moto informada pelo ID. O corpo da requisição deve conter o modelo <see cref="MotoDTO"/>.
        /// O campo <b>ProblemaIdentificado</b> é opcional.
        /// </remarks>
        /// <param name="id">Identificador único da moto</param>
        /// <param name="dto">Dados da moto a serem atualizados</param>
        /// <response code="204">Moto atualizada com sucesso (No Content)</response>
        /// <response code="400">Alguma restrição de negócio foi violada</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(MotoDTO), typeof(GeoSense.API.Examples.MotoDTOExample))]
        [SwaggerResponse(204, "Moto atualizada com sucesso (No Content)")]
        [SwaggerResponse(400, "Restrição de negócio violada")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> PutMoto(long id, MotoDTO dto)
        {
            var moto = await _service.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound();

            // Validações de negócio
            var motos = await _service.ObterTodasAsync();

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

            await _service.AtualizarAsync(moto);

            return NoContent();
        }

        /// <summary>
        /// Cadastra uma nova moto.
        /// </summary>
        /// <remarks>
        /// Cadastra uma nova moto no sistema. O corpo da requisição deve conter o modelo <see cref="MotoDTO"/>.
        /// O campo <b>ProblemaIdentificado</b> é opcional.
        /// </remarks>
        /// <param name="dto">Dados da nova moto</param>
        /// <response code="201">Moto criada com sucesso</response>
        /// <response code="400">Alguma restrição de negócio foi violada</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(MotoDTO), typeof(GeoSense.API.Examples.MotoDTOExample))]
        [SwaggerResponse(201, "Moto criada com sucesso", typeof(object))]
        [SwaggerResponse(400, "Restrição de negócio violada")]
        public async Task<ActionResult<MotoDetalhesDTO>> PostMoto(MotoDTO dto)
        {
            var motos = await _service.ObterTodasAsync();

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

            await _service.AdicionarAsync(novaMoto);

            var motoCompleta = await _service.ObterPorIdAsync(novaMoto.Id);

            var resultDto = _mapper.Map<MotoDetalhesDTO>(motoCompleta);

            return CreatedAtAction(nameof(GetMoto), new { id = novaMoto.Id }, new
            {
                mensagem = "Moto cadastrada com sucesso.",
                dados = resultDto
            });
        }

        /// <summary>
        /// Exclui uma moto do sistema.
        /// </summary>
        /// <remarks>
        /// Remove a moto informada pelo ID.
        /// </remarks>
        /// <param name="id">Identificador único da moto</param>
        /// <response code="204">Moto removida com sucesso (No Content)</response>
        /// <response code="404">Moto não encontrada</response>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Moto removida com sucesso (No Content)")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> DeleteMoto(long id)
        {
            var moto = await _service.ObterPorIdAsync(id);
            if (moto == null)
                return NotFound();

            await _service.RemoverAsync(moto);

            return NoContent();
        }
    }
}