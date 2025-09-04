using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.Infrastructure.Repositories.Interfaces
{
    public interface IMotoRepository
    {
        Task<List<Moto>> ObterTodasAsync();
        Task<Moto> ObterPorIdComVagaEDefeitosAsync(long id);
    }
}