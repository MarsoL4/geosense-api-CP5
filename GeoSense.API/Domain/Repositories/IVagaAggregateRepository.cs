namespace GeoSense.API.Domain.Repositories
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GeoSense.API.Domain.Aggregates;

    /// <summary>
    /// Repositório de agregado para Vaga. Retorna/recebe o agregado (VagaAggregate)
    /// e encapsula operações que refletem regras de negócio do agregado.
    /// </summary>
    public interface IVagaAggregateRepository
    {
        Task<List<VagaAggregate>> ObterTodasAsync();
        Task<VagaAggregate?> ObterPorIdAsync(long id);
        Task<VagaAggregate> AdicionarAsync(VagaAggregate vagaAggregate);
        Task AtualizarAsync(VagaAggregate vagaAggregate);
        Task RemoverAsync(VagaAggregate vagaAggregate);
    }
}