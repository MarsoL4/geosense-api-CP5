namespace GeoSense.API.Domain.Repositories
{
    using GeoSense.API.Infrastructure.Persistence;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IMotoRepository
    {
        Task<List<Moto>> ObterTodasAsync();
        Task<Moto?> ObterPorIdComVagaEDefeitosAsync(long id);
        Task<Moto> AdicionarAsync(Moto moto);
        Task AtualizarAsync(Moto moto);
        Task RemoverAsync(Moto moto);
    }
}