using GeoSense.API.src.Domain.Entities;

namespace GeoSense.API.src.Domain.Repositories.Interfaces
{
    public interface IMotoRepository
    {
        Task<List<Moto>> ObterTodasAsync();
        Task<Moto> ObterPorIdComVagaEDefeitosAsync(long id);
    }
}