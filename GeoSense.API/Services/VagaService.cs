using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs.Vaga;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;

namespace GeoSense.API.Services
{
    /// <summary>
    /// Serviço de regras de negócio para vagas.
    /// </summary>
    public class VagaService(IVagaRepository repo)
    {
        private readonly IVagaRepository _repo = repo;

        /// <summary>
        /// Retorna todas as vagas cadastradas.
        /// </summary>
        public async Task<List<Vaga>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        /// <summary>
        /// Retorna os detalhes de uma vaga pelo id.
        /// </summary>
        public async Task<Vaga?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        /// <summary>
        /// Adiciona uma nova vaga.
        /// </summary>
        public async Task<Vaga> AdicionarAsync(Vaga vaga) => await _repo.AdicionarAsync(vaga);

        /// <summary>
        /// Atualiza os dados de uma vaga existente.
        /// </summary>
        public async Task AtualizarAsync(Vaga vaga) => await _repo.AtualizarAsync(vaga);

        /// <summary>
        /// Remove uma vaga do sistema.
        /// </summary>
        public async Task RemoverAsync(Vaga vaga) => await _repo.RemoverAsync(vaga);

        /// <summary>
        /// Retorna dados agregados sobre vagas livres.
        /// </summary>
        public async Task<VagasStatusDTO> ObterVagasLivresAsync()
        {
            var vagasLivres = (await _repo.ObterTodasAsync()).Where(v => v.Status == StatusVaga.LIVRE).Select(v => v.Tipo).ToList();
            var livresComProblema = vagasLivres.Count(tipo => tipo != TipoVaga.Sem_Problema);
            var livresSemProblema = vagasLivres.Count(tipo => tipo == TipoVaga.Sem_Problema);

            return new VagasStatusDTO
            {
                LivresComProblema = livresComProblema,
                LivresSemProblema = livresSemProblema
            };
        }
    }
}