using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoSense.API.Infrastructure.Repositories
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