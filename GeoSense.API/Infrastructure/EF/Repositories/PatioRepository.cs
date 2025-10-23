using GeoSense.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using GeoSense.API.Domain.Entities;
using GeoSense.API.Infrastructure.EF.Contexts;

namespace GeoSense.API.Infrastructure.EF.Repositories
{
    public class PatioRepository : IPatioRepository
    {
        private readonly GeoSenseContext _context;

        public PatioRepository(GeoSenseContext context)
        {
            _context = context;
        }

        public async Task<List<Patio>> ObterTodasAsync()
        {
            return await _context.Patios.Include(p => p.Vagas).ToListAsync();
        }

        public async Task<Patio?> ObterPorIdAsync(long id)
        {
            return await _context.Patios
                .Include(p => p.Vagas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patio> AdicionarAsync(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();
            return patio;
        }

        public async Task AtualizarAsync(Patio patio)
        {
            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Patio patio)
        {
            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
        }
    }
}