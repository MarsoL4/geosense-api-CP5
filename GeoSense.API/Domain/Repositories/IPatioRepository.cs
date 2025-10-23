namespace GeoSense.API.Domain.Repositories
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GeoSense.API.Domain.Entities;

    public interface IPatioRepository
    {
        Task<List<Patio>> ObterTodasAsync();
        Task<Patio?> ObterPorIdAsync(long id);
        Task<Patio> AdicionarAsync(Patio patio);
        Task AtualizarAsync(Patio patio);
        Task RemoverAsync(Patio patio);
    }
}