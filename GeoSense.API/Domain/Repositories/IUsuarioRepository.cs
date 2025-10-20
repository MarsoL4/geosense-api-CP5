namespace GeoSense.API.Domain.Repositories
{
    using GeoSense.API.Infrastructure.Persistence;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ObterTodasAsync();
        Task<Usuario?> ObterPorIdAsync(long id);
        Task<Usuario> AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(Usuario usuario);
        Task<bool> EmailExisteAsync(string email, long? ignoreId = null);
    }
}