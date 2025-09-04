using GeoSense.API.src.Domain.Entities;

namespace GeoSense.API.src.Domain.Repositories.Interfaces
{
    public interface IMotoRepository
    {
        Task<List<Moto>> ObterTodasAsync();
        Task<Moto> ObterPorIdComVagaEDefeitosAsync(long id);
        Task AdicionarAsync(Moto moto);
        Task AtualizarAsync(Moto moto);
        Task RemoverAsync(Moto moto);
    }
}