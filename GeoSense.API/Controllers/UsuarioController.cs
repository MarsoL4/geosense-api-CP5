using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs;
using GeoSense.API.DTOs.Usuario;
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
    public class UsuarioController(UsuarioService service, IMapper mapper) : ControllerBase
    {
        private readonly UsuarioService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Retorna uma lista paginada de usuários cadastrados.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
        /// <response code="200">Lista paginada de usuários</response>
        [HttpGet]
        [SwaggerResponse(200, "Lista paginada de usuários cadastrados", typeof(PagedHateoasDTO<UsuarioDTO>))]
        public async Task<ActionResult<PagedHateoasDTO<UsuarioDTO>>> GetUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = await _service.ObterTodasAsync();
            var totalCount = usuarios.Count;
            var paged = usuarios.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var items = _mapper.Map<List<UsuarioDTO>>(paged);

            var links = HateoasHelper.GetPagedLinks(Url, "Usuarios", page, pageSize, totalCount);

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
        /// <param name="id">Identificador único do usuário</param>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id:long}")]
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
        /// <param name="email">E-mail do usuário</param>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{email}")]
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
                Senha = usuario.Senha
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <param name="dto">Dados do novo usuário</param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Email já cadastrado</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(GeoSense.API.Examples.UsuarioDTOExample))]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(object))]
        [SwaggerResponse(400, "Email já cadastrado")]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO dto)
        {
            var emailExiste = await _service.EmailExisteAsync(dto.Email);

            if (emailExiste)
                return BadRequest(new { mensagem = "Já existe um usuário com esse email." });

            var novoUsuario = new GeoSense.API.Infrastructure.Persistence.Usuario(0, dto.Nome, dto.Email, dto.Senha, (TipoUsuario)dto.Tipo);
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
        /// <param name="id">Identificador único do usuário</param>
        /// <param name="dto">Dados do usuário a serem atualizados</param>
        /// <response code="204">Usuário atualizado com sucesso (No Content)</response>
        /// <response code="400">Email já cadastrado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(UsuarioDTO), typeof(GeoSense.API.Examples.UsuarioDTOExample))]
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
        /// <param name="id">Identificador único do usuário</param>
        /// <response code="204">Usuário removido com sucesso (No Content)</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
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