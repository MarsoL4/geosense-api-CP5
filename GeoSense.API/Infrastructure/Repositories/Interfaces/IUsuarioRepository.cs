using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface para acesso a dados de Usuario.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        Task<List<Usuario>> ObterTodasAsync();
        /// <summary>
        /// Retorna um usuário pelo id.
        /// </summary>
        Task<Usuario?> ObterPorIdAsync(long id);
        /// <summary>
        /// Adiciona um novo usuário.
        /// </summary>
        Task<Usuario> AdicionarAsync(Usuario usuario);
        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        Task AtualizarAsync(Usuario usuario);
        /// <summary>
        /// Remove um usuário do sistema.
        /// </summary>
        Task RemoverAsync(Usuario usuario);
        /// <summary>
        /// Verifica se um e-mail já existe no cadastro de usuários.
        /// </summary>
        Task<bool> EmailExisteAsync(string email, long? ignoreId = null);
    }
}