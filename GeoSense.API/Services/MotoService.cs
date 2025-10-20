using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoSense.API.Services
{
    public class MotoService
    {
        private readonly IMotoRepository _repo;

        public MotoService(IMotoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Moto>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        public async Task<Moto?> ObterPorIdAsync(long id) => await _repo.ObterPorIdComVagaEDefeitosAsync(id);

        public async Task<Moto> AdicionarAsync(Moto moto) => await _repo.AdicionarAsync(moto);

        public async Task AtualizarAsync(Moto moto) => await _repo.AtualizarAsync(moto);

        public async Task RemoverAsync(Moto moto) => await _repo.RemoverAsync(moto);
    }
}