using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeoSense.API.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Usuario>> ObterTodasAsync() => await _repo.ObterTodasAsync();

        public async Task<Usuario?> ObterPorIdAsync(long id) => await _repo.ObterPorIdAsync(id);

        public async Task<Usuario> AdicionarAsync(Usuario usuario) => await _repo.AdicionarAsync(usuario);

        public async Task AtualizarAsync(Usuario usuario) => await _repo.AtualizarAsync(usuario);

        public async Task RemoverAsync(Usuario usuario) => await _repo.RemoverAsync(usuario);

        public async Task<bool> EmailExisteAsync(string email, long? ignoreId = null) => await _repo.EmailExisteAsync(email, ignoreId);
    }
}