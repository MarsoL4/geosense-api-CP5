using GeoSense.API.src.Domain.Entities;
using GeoSense.API.src.Domain.Repositories.Interfaces;
using GeoSense.API.src.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.src.Infrastructure.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly GeoSenseContext _context;

        public MotoRepository(GeoSenseContext context)
        {
            _context = context;
        }

        public async Task<List<Moto>> ObterTodasAsync()
        {
            return await _context.Motos
                .Include(m => m.Vaga)
                .ToListAsync();
        }

        public async Task<Moto> ObterPorIdComVagaEDefeitosAsync(long id)
        {
            return await _context.Motos
                .Include(m => m.Vaga)
                .Include(m => m.Defeitos)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}