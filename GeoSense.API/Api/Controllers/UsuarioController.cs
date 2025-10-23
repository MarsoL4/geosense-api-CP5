using AutoMapper;
using GeoSense.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using GeoSense.API.Application.DTOs;
using GeoSense.API.Application.DTOs.Usuario;
using GeoSense.API.Application.Services;
using GeoSense.API.Api.Helpers;
using GeoSense.API.Domain.Entities;

namespace GeoSense.API.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsuarioController(UsuarioService service, IMapper mapper) : ControllerBase
    {
        private readonly UsuarioService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de usuários cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista paginada de usuários", Description = "Retorna uma página com usuários cadastrados.")]
        [SwaggerResponse(200, "Lista paginada de usuários cadastrados", typeof(PagedHateoasDTO<UsuarioDTO>))]
        public async Task<ActionResult<PagedHateoasDTO<UsuarioDTO>>> GetUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = await _service.ObterTodasAsync();
            var totalCount = usuarios.Count;
            var paged = usuarios.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<UsuarioDTO>>(paged);

            // padronizado
            var links = HateoasHelper.GetPagedLinks(Url, "usuario", page, pageSize, totalCount);

            var result = new PagedHateoasDTO<UsuarioDTO>
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
        /// Retorna os dados de um usuário por ID.
        /// </summary>
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Buscar usuário por ID", Description = "Retorna os dados de um usuário a partir do seu identificador.")]
        [SwaggerResponse(200, "Usuário encontrado", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(long id)
        {
            var usuario = await _service.ObterPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            var dto = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(dto);
        }

        /// <summary>
        /// Retorna os dados de um usuário por email.
        /// </summary>
        [HttpGet("{email}")]
        [SwaggerOperation(Summary = "Buscar usuário por email", Description = "Busca usuário pelo email (único).")]
        [SwaggerResponse(200, "Usuário encontrado por email", typeof(UsuarioDTO))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioPorEmail(string email)
        {
            var usuarios = await _service.ObterTodasAsync();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            // Retornando os campos solicitados
            var dto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Tipo = (int)usuario.Tipo
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar usuário", Description = "Cadastra um novo usuário no sistema.")]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(Examples.UsuarioDTOExample))]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(object))]
        [SwaggerResponse(400, "Email já cadastrado")]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO dto)
        {
            var emailExiste = await _service.EmailExisteAsync(dto.Email);

            if (emailExiste)
                return BadRequest(new { mensagem = "Já existe um usuário com esse email." });

            // Use AutoMapper to create Usuario instance (keeps controller decoupled from persistence)
            var novoUsuario = _mapper.Map<Usuario>(dto);
            await _service.AdicionarAsync(novoUsuario);

            var usuarioCompleto = await _service.ObterPorIdAsync(novoUsuario.Id);
            var resultDto = _mapper.Map<UsuarioDTO>(usuarioCompleto);

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, new
            {
                mensagem = "Usuário cadastrado com sucesso.",
                dados = resultDto
            });
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar usuário", Description = "Atualiza os dados de um usuário existente.")]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(Examples.UsuarioDTOExample))]
        [SwaggerResponse(204, "Usuário atualizado com sucesso (No Content)")]
        [SwaggerResponse(400, "Email já cadastrado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> PutUsuario(long id, UsuarioDTO dto)
        {
            var usuario = await _service.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            var emailExiste = await _service.EmailExisteAsync(dto.Email, id);

            if (emailExiste)
                return BadRequest(new { mensagem = "Já existe um usuário com esse email." });

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = dto.Senha;
            usuario.Tipo = (TipoUsuario)dto.Tipo;

            await _service.AtualizarAsync(usuario);

            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário do sistema.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover usuário", Description = "Remove o usuário identificado pelo ID.")]
        [SwaggerResponse(204, "Usuário removido com sucesso (No Content)")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            var usuario = await _service.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            await _service.RemoverAsync(usuario);
            return NoContent();
        }
    }
}