using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Domain.Enums;
using GeoSense.API.DTOs.Vaga;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GeoSense.API.Services
{
    public class VagaService
    {
        private readonly IVagaRepository _repo;

        public VagaService(IVagaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Vaga>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        public async Task<Vaga?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        public async Task<Vaga> AdicionarAsync(Vaga vaga) => await _repo.AdicionarAsync(vaga);

        public async Task AtualizarAsync(Vaga vaga) => await _repo.AtualizarAsync(vaga);

        public async Task RemoverAsync(Vaga vaga) => await _repo.RemoverAsync(vaga);

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