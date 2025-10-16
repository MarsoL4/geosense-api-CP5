using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Repositories
{
    public class VagaRepository(GeoSenseContext context) : IVagaRepository
    {
        private readonly GeoSenseContext _context = context;

        public async Task<List<Vaga>> ObterTodasAsync()
        {
            return await _context.Vagas.Include(v => v.Motos).ToListAsync();
        }

        public async Task<Vaga?> ObterPorIdAsync(long id)
        {
            return await _context.Vagas
                .Include(v => v.Motos)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vaga> AdicionarAsync(Vaga vaga)
        {
            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();
            return vaga;
        }

        public async Task AtualizarAsync(Vaga vaga)
        {
            _context.Vagas.Update(vaga);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Vaga vaga)
        {
            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();
        }
    }
}