using GeoSense.API.Domain.Aggregates;
using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GeoSense.API.Infrastructure.Adapters
{
    /// <summary>
    /// Adapter que implementa IVagaAggregateRepository persistindo/consultando via VagaMongoRepository.
    /// Usa a classe de domínio VagaAggregate como contrato na aplicação.
    /// </summary>
    public class VagaAggregateMongoRepository(VagaMongoRepository vagaRepo, MotoMongoRepository motoRepo) : IVagaAggregateRepository
    {
        private readonly VagaMongoRepository _vagaRepo = vagaRepo;
        private readonly MotoMongoRepository _motoRepo = motoRepo;

        public async Task<List<VagaAggregate>> ObterTodasAsync()
        {
            var vagas = await _vagaRepo.ObterTodasAsync();
            return [.. vagas.Select(VagaAggregate.FromPersistence)];
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

            // Sincronizar moto (se houve alocação no agregado)
            if (vagaAggregate.MotoId.HasValue)
            {
                var moto = await _motoRepo.ObterPorIdAsync(vagaAggregate.MotoId.Value);
                if (moto != null)
                {
                    moto.VagaId = inserted.Id;
                    await _motoRepo.AtualizarAsync(moto);
                }
            }

            return VagaAggregate.FromPersistence(inserted);
        }

        public async Task AtualizarAsync(VagaAggregate vagaAggregate)
        {
            var existing = await _vagaRepo.ObterPorIdAsync(vagaAggregate.Id) ?? throw new InvalidOperationException("Vaga não encontrada.");

            // Id da moto previamente alocada (se houver)
            var previousMotoId = existing.Motos?.FirstOrDefault()?.Id;

            // Aplica alterações do agregado na entidade existente
            vagaAggregate.ApplyToPersistence(existing);

            await _vagaRepo.AtualizarAsync(existing);

            // Se houve mudança na alocação, atualiza os documentos de Moto correspondentes
            var newMotoId = vagaAggregate.MotoId;

            if (previousMotoId != newMotoId)
            {
                // Limpar VagaId da moto anteriormente alocada (se houver)
                if (previousMotoId.HasValue)
                {
                    try
                    {
                        var prevMoto = await _motoRepo.ObterPorIdAsync(previousMotoId.Value);
                        if (prevMoto != null)
                        {
                            // Indica "sem vaga" - o projeto usa 0 em alguns lugares; mantenha consistente
                            prevMoto.VagaId = 0;
                            await _motoRepo.AtualizarAsync(prevMoto);
                        }
                    }
                    catch
                    {
                        // não falhar o fluxo principal por problema ao sincronizar a moto anterior
                    }
                }

                // Atribuir VagaId na nova moto alocada (se houver)
                if (newMotoId.HasValue)
                {
                    try
                    {
                        var newMoto = await _motoRepo.ObterPorIdAsync(newMotoId.Value);
                        if (newMoto != null)
                        {
                            newMoto.VagaId = vagaAggregate.Id;
                            await _motoRepo.AtualizarAsync(newMoto);
                        }
                    }
                    catch
                    {
                        // não falhar o fluxo principal por problema ao sincronizar a nova moto
                    }
                }
            }
        }

        public async Task RemoverAsync(VagaAggregate vagaAggregate)
        {
            var existing = await _vagaRepo.ObterPorIdAsync(vagaAggregate.Id) ?? throw new InvalidOperationException("Vaga não encontrada.");

            // Antes de remover, se havia moto alocada, limpar VagaId dela
            var allocatedMotoId = existing.Motos?.FirstOrDefault()?.Id;
            if (allocatedMotoId.HasValue)
            {
                var moto = await _motoRepo.ObterPorIdAsync(allocatedMotoId.Value);
                if (moto != null)
                {
                    moto.VagaId = 0;
                    await _motoRepo.AtualizarAsync(moto);
                }
            }

            await _vagaRepo.RemoverAsync(existing);
        }
    }
}