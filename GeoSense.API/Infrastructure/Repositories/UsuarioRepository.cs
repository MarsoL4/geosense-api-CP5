using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Persistence;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Repositories
{
    public class UsuarioRepository(GeoSenseContext context) : IUsuarioRepository
    {
        private readonly GeoSenseContext _context = context;

        public async Task<List<Usuario>> ObterTodasAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(long id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExisteAsync(string email, long? ignoreId = null)
        {
            return await _context.Usuarios
                .CountAsync(u => u.Email == email && (!ignoreId.HasValue || u.Id != ignoreId.Value)) > 0;
        }
    }
}