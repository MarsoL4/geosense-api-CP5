using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface para acesso a dados de Patio.
    /// </summary>
    public interface IPatioRepository
    {
        /// <summary>
        /// Retorna todos os pátios cadastrados.
        /// </summary>
        Task<List<Patio>> ObterTodasAsync();
        /// <summary>
        /// Retorna um pátio pelo id.
        /// </summary>
        Task<Patio?> ObterPorIdAsync(long id);
        /// <summary>
        /// Adiciona um novo pátio.
        /// </summary>
        Task<Patio> AdicionarAsync(Patio patio);
        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        Task AtualizarAsync(Patio patio);
        /// <summary>
        /// Remove um pátio do sistema.
        /// </summary>
        Task RemoverAsync(Patio patio);
    }
}