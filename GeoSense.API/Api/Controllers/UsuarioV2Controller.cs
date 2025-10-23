using AutoMapper;
using GeoSense.API.Api.Helpers;
using GeoSense.API.Application.DTOs.Usuario;
using GeoSense.API.Domain.Entities;
using GeoSense.API.Infrastructure.Mongo;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace GeoSense.API.Api.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/usuario")]
    [ApiController]
    [Produces("application/json")]
    public class UsuarioV2Controller : ControllerBase
    {
        private readonly UsuarioMongoRepository _repo;
        private readonly IMapper _mapper;

        public UsuarioV2Controller(UsuarioMongoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma lista paginada de usuários cadastrados (Mongo).
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de usuários (Mongo)", Description = "Retorna uma página com usuários armazenados no MongoDB.")]
        [SwaggerResponse(200, "Lista paginada de usuários (Mongo)", typeof(object))]
        public async Task<ActionResult> GetUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = await _repo.ObterTodasAsync();
            var totalCount = usuarios.Count;
            var paged = usuarios.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<UsuarioDTO>>(paged);

            // padronizado
            var links = HateoasHelper.GetPagedLinks(Url, "usuario", page, pageSize, totalCount);

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
        /// Retorna os dados de um usuário por ID (Mongo).
        /// </summary>
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Buscar usuário por ID (Mongo)", Description = "Retorna os dados de um usuário no MongoDB por id.")]
        [SwaggerResponse(200, "Usuário encontrado (Mongo)", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(long id)
        {
            var usuario = await _repo.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            var dto = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(dto);
        }

        /// <summary>
        /// Retorna os dados de um usuário por email (Mongo).
        /// </summary>
        [HttpGet("by-email/{email}")]
        [SwaggerOperation(Summary = "Buscar usuário por email (Mongo)", Description = "Busca usuário pelo email no MongoDB.")]
        [SwaggerResponse(200, "Usuário encontrado por email", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioPorEmail(string email)
        {
            var usuarios = await _repo.ObterTodasAsync();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            var dto = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(dto);
        }

        /// <summary>
        /// Cadastra um novo usuário (Mongo).
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar usuário (Mongo)", Description = "Cadastra um novo usuário no MongoDB.")]
        [SwaggerResponse(201, "Usuário criado com sucesso (Mongo)")]
        [SwaggerResponse(400, "Email já cadastrado")]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(Examples.UsuarioDTOExample))]
        public async Task<ActionResult> PostUsuario(UsuarioDTO dto)
        {
            var emailExiste = await _repo.EmailExisteAsync(dto.Email);
            if (emailExiste)
                return BadRequest(new { mensagem = "Já existe um usuário com esse email." });

            // usar AutoMapper
            var novoUsuario = _mapper.Map<Usuario>(dto);

            await _repo.AdicionarAsync(novoUsuario);

            var usuarioCompleto = await _repo.ObterPorIdAsync(novoUsuario.Id);
            var resultDto = _mapper.Map<UsuarioDTO>(usuarioCompleto);

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id, version = "2.0" }, new
            {
                mensagem = "Usuário cadastrado com sucesso (Mongo).",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente (Mongo).
        /// </summary>
        [HttpPut("{id:long}")]
        [SwaggerOperation(Summary = "Atualizar usuário (Mongo)", Description = "Atualiza um usuário existente no MongoDB.")]
        [SwaggerResponse(204, "Usuário atualizado com sucesso (Mongo)")]
        [SwaggerResponse(400, "Email já cadastrado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(Examples.UsuarioDTOExample))]
        public async Task<IActionResult> PutUsuario(long id, UsuarioDTO dto)
        {
            var usuario = await _repo.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            var emailExiste = await _repo.EmailExisteAsync(dto.Email, id);
            if (emailExiste)
                return BadRequest(new { mensagem = "Já existe um usuário com esse email." });

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = dto.Senha;
            usuario.Tipo = (Domain.Enums.TipoUsuario)dto.Tipo;

            await _repo.AtualizarAsync(usuario);

            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário do sistema (Mongo).
        /// </summary>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Remover usuário (Mongo)", Description = "Remove usuário do MongoDB pelo ID.")]
        [SwaggerResponse(204, "Usuário removido com sucesso (Mongo)")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            var usuario = await _repo.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            await _repo.RemoverAsync(usuario);
            return NoContent();
        }
    }
}