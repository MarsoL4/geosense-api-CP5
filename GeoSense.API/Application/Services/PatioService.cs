using GeoSense.API.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using GeoSense.API.Domain.Entities;

namespace GeoSense.API.Application.Services
{
    public class PatioService
    {
        private readonly IPatioRepository _repo;

        public PatioService(IPatioRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Patio>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        public async Task<Patio?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        public async Task<Patio> AdicionarAsync(Patio patio) => await _repo.AdicionarAsync(patio);

        public async Task AtualizarAsync(Patio patio) => await _repo.AtualizarAsync(patio);

        public async Task RemoverAsync(Patio patio) => await _repo.RemoverAsync(patio);
    }
}