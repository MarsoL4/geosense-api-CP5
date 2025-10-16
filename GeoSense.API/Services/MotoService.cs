using GeoSense.API.DTOs.Moto;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;

namespace GeoSense.API.Services
{
    /// <summary>
    /// Serviço de regras de negócio para motos.
    /// </summary>
    public class MotoService(IMotoRepository repo)
    {
        private readonly IMotoRepository _repo = repo;

        /// <summary>
        /// Retorna todas as motos cadastradas.
        /// </summary>
        public async Task<List<Moto>> ObterTodasAsync()
        {
            return await _repo.ObterTodasAsync();
        }

        /// <summary>
        /// Retorna os detalhes de uma moto pelo id.
        /// </summary>
        public async Task<Moto?> ObterPorIdAsync(long id)
        {
            return await _repo.ObterPorIdComVagaEDefeitosAsync(id);
        }

        /// <summary>
        /// Adiciona uma nova moto.
        /// </summary>
        public async Task<Moto> AdicionarAsync(Moto moto)
        {
            return await _repo.AdicionarAsync(moto);
        }

        /// <summary>
        /// Atualiza os dados de uma moto existente.
        /// </summary>
        public async Task AtualizarAsync(Moto moto)
        {
            await _repo.AtualizarAsync(moto);
        }

        /// <summary>
        /// Remove uma moto do sistema.
        /// </summary>
        public async Task RemoverAsync(Moto moto)
        {
            await _repo.RemoverAsync(moto);
        }
    }
}