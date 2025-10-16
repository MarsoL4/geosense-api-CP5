using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Repositories
{
    public class MotoRepository(GeoSenseContext context) : IMotoRepository
    {
        private readonly GeoSenseContext _context = context;

        public async Task<List<Moto>> ObterTodasAsync()
        {
            return await _context.Motos
                .Include(m => m.Vaga)
                .ToListAsync();
        }

        public async Task<Moto?> ObterPorIdComVagaEDefeitosAsync(long id)
        {
            return await _context.Motos
                .Include(m => m.Vaga)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Moto> AdicionarAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task AtualizarAsync(Moto moto)
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Moto moto)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
        }
    }
}