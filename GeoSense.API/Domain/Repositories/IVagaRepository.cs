namespace GeoSense.API.Domain.Repositories
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GeoSense.API.Domain.Entities;

    public interface IVagaRepository
    {
        Task<List<Vaga>> ObterTodasAsync();
        Task<Vaga?> ObterPorIdAsync(long id);
        Task<Vaga> AdicionarAsync(Vaga vaga);
        Task AtualizarAsync(Vaga vaga);
        Task RemoverAsync(Vaga vaga);
    }
}