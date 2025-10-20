using GeoSense.API.Domain.Aggregates;
using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Adapters
{
    /// <summary>
    /// Adapter que implementa IVagaAggregateRepository persistindo/consultando via VagaMongoRepository.
    /// Usa a classe de domínio VagaAggregate como contrato na aplicação.
    /// </summary>
    public class VagaAggregateMongoRepository : IVagaAggregateRepository
    {
        private readonly VagaMongoRepository _vagaRepo;
        private readonly MotoMongoRepository _motoRepo;

        public VagaAggregateMongoRepository(VagaMongoRepository vagaRepo, MotoMongoRepository motoRepo)
        {
            _vagaRepo = vagaRepo;
            _motoRepo = motoRepo;
        }

        public async Task<List<VagaAggregate>> ObterTodasAsync()
        {
            var vagas = await _vagaRepo.ObterTodasAsync();
            return vagas.Select(VagaAggregate.FromPersistence).ToList();
        }

        public async Task<VagaAggregate?> ObterPorIdAsync(long id)
        {
            var vaga = await _vagaRepo.ObterPorIdAsync(id);
            if (vaga == null) return null;
            return VagaAggregate.FromPersistence(vaga);
        }

        public async Task<VagaAggregate> AdicionarAsync(VagaAggregate vagaAggregate)
        {
            // Cria uma entidade de persistência, aplica agregados e persiste
            var vaga = new Vaga(vagaAggregate.Numero, vagaAggregate.PatioId)
            {
                Id = vagaAggregate.Id,
                Tipo = (GeoSense.API.Domain.Enums.TipoVaga)vagaAggregate.Tipo,
                Status = (GeoSense.API.Domain.Enums.StatusVaga)vagaAggregate.Status
            };

            // Aplique dados do agregado para a entidade (usa o método de domínio)
            vagaAggregate.ApplyToPersistence(vaga);

            var inserted = await _vagaRepo.AdicionarAsync(vaga);
            return VagaAggregate.FromPersistence(inserted);
        }

        public async Task AtualizarAsync(VagaAggregate vagaAggregate)
        {
            var existing = await _vagaRepo.ObterPorIdAsync(vagaAggregate.Id);
            if (existing == null) throw new InvalidOperationException("Vaga não encontrada.");

            // Aplica alterações do agregado na entidade existente
            vagaAggregate.ApplyToPersistence(existing);

            await _vagaRepo.AtualizarAsync(existing);
        }

        public async Task RemoverAsync(VagaAggregate vagaAggregate)
        {
            var existing = await _vagaRepo.ObterPorIdAsync(vagaAggregate.Id);
            if (existing == null) throw new InvalidOperationException("Vaga não encontrada.");

            await _vagaRepo.RemoverAsync(existing);
        }
    }
}