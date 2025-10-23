using GeoSense.API.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using GeoSense.API.Domain.Entities;

namespace GeoSense.API.Application.Services
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