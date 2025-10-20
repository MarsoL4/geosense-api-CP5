namespace GeoSense.API.Domain.Repositories
{
    using GeoSense.API.Infrastructure.Persistence;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IPatioRepository
    {
        Task<List<Patio>> ObterTodasAsync();
        Task<Patio?> ObterPorIdAsync(long id);
        Task<Patio> AdicionarAsync(Patio patio);
        Task AtualizarAsync(Patio patio);
        Task RemoverAsync(Patio patio);
    }
}