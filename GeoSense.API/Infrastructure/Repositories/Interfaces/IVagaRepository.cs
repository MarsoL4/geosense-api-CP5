using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface para acesso a dados de Vaga.
    /// </summary>
    public interface IVagaRepository
    {
        /// <summary>
        /// Retorna todas as vagas cadastradas.
        /// </summary>
        Task<List<Vaga>> ObterTodasAsync();
        /// <summary>
        /// Retorna uma vaga pelo id.
        /// </summary>
        Task<Vaga?> ObterPorIdAsync(long id);
        /// <summary>
        /// Adiciona uma nova vaga.
        /// </summary>
        Task<Vaga> AdicionarAsync(Vaga vaga);
        /// <summary>
        /// Atualiza os dados de uma vaga existente.
        /// </summary>
        Task AtualizarAsync(Vaga vaga);
        /// <summary>
        /// Remove uma vaga do sistema.
        /// </summary>
        Task RemoverAsync(Vaga vaga);
    }
}