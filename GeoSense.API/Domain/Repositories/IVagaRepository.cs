namespace GeoSense.API.Domain.Repositories
{
    using GeoSense.API.Infrastructure.Persistence;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IVagaRepository
    {
        Task<List<Vaga>> ObterTodasAsync();
        Task<Vaga?> ObterPorIdAsync(long id);
        Task<Vaga> AdicionarAsync(Vaga vaga);
        Task AtualizarAsync(Vaga vaga);
        Task RemoverAsync(Vaga vaga);
    }
}