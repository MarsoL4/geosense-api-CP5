using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface para acesso a dados de Moto.
    /// </summary>
    public interface IMotoRepository
    {
        /// <summary>
        /// Retorna todas as motos cadastradas.
        /// </summary>
        Task<List<Moto>> ObterTodasAsync();
        /// <summary>
        /// Retorna uma moto pelo id, incluindo sua vaga.
        /// </summary>
        Task<Moto?> ObterPorIdComVagaEDefeitosAsync(long id);
        /// <summary>
        /// Adiciona uma nova moto.
        /// </summary>
        Task<Moto> AdicionarAsync(Moto moto);
        /// <summary>
        /// Atualiza os dados de uma moto existente.
        /// </summary>
        Task AtualizarAsync(Moto moto);
        /// <summary>
        /// Remove uma moto do sistema.
        /// </summary>
        Task RemoverAsync(Moto moto);
    }
}