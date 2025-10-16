using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;

namespace GeoSense.API.Services
{
    /// <summary>
    /// Serviço de regras de negócio para usuários.
    /// </summary>
    public class UsuarioService(IUsuarioRepository repo)
    {
        private readonly IUsuarioRepository _repo = repo;

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        public async Task<List<Usuario>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        /// <summary>
        /// Retorna os detalhes de um usuário pelo id.
        /// </summary>
        public async Task<Usuario?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        public async Task<Usuario> AdicionarAsync(Usuario usuario) => await _repo.AdicionarAsync(usuario);

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        public async Task AtualizarAsync(Usuario usuario) => await _repo.AtualizarAsync(usuario);

        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        public async Task RemoverAsync(Usuario usuario) => await _repo.RemoverAsync(usuario);

        /// <summary>
        /// Verifica se um e-mail já existe no cadastro de usuários.
        /// </summary>
        public async Task<bool> EmailExisteAsync(string email, long? ignoreId = null) => await _repo.EmailExisteAsync(email, ignoreId);
    }
}