using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;

namespace GeoSense.API.Services
{
    /// <summary>
    /// Serviço de regras de negócio para pátios.
    /// </summary>
    public class PatioService(IPatioRepository repo)
    {
        private readonly IPatioRepository _repo = repo;

        /// <summary>
        /// Retorna todos os pátios cadastrados.
        /// </summary>
        public async Task<List<Patio>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        /// <summary>
        /// Retorna os detalhes de um pátio pelo id.
        /// </summary>
        public async Task<Patio?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        /// <summary>
        /// Adiciona um novo pátio.
        /// </summary>
        public async Task<Patio> AdicionarAsync(Patio patio) => await _repo.AdicionarAsync(patio);

        /// <summary>
        /// Atualiza os dados de um pátio existente.
        /// </summary>
        public async Task AtualizarAsync(Patio patio) => await _repo.AtualizarAsync(patio);

        /// <summary>
        /// Remove um pátio do sistema.
        /// </summary>
        public async Task RemoverAsync(Patio patio) => await _repo.RemoverAsync(patio);
    }
}