using System;
using System.Threading.Tasks;
using Xunit;
using Mongo2Go;
using MongoDB.Driver;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Domain.Entities;

namespace GeoSense.API.Tests.Integration
{
    public class MongoIntegrationTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoClient _client;
        private readonly MongoSettings _settings;

        public MongoIntegrationTests()
        {
            // Inicia Mongo temporário
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);
            _client = new MongoClient(_runner.ConnectionString);

            _settings = new MongoSettings
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = $"geosense_test_{Guid.NewGuid():N}"
            };
        }

        [Fact(DisplayName = "MotoMongoRepository_CRUD")]
        public async Task MotoMongoRepository_CRUD()
        {
            var repo = new MotoMongoRepository(_client, _settings);

            // Create (Adicionar)
            var moto = new Moto
            {
                Modelo = "Teste Integracao",
                Placa = $"INT{DateTime.UtcNow.Ticks % 10000}",
                Chassi = $"CHASSI{Guid.NewGuid():N}".Substring(0, 20),
                ProblemaIdentificado = "Nenhum",
                VagaId = 1
            };

            var inserted = await repo.AdicionarAsync(moto);
            Assert.True(inserted.Id != 0);

            // Read (ObterPorId)
            var fetched = await repo.ObterPorIdAsync(inserted.Id);
            Assert.NotNull(fetched);
            Assert.Equal(inserted.Placa, fetched!.Placa);

            // Update
            fetched.Modelo = "Atualizado";
            await repo.AtualizarAsync(fetched);

            var updated = await repo.ObterPorIdAsync(fetched.Id);
            Assert.Equal("Atualizado", updated!.Modelo);

            // Delete
            await repo.RemoverAsync(updated);
            var deleted = await repo.ObterPorIdAsync(updated.Id);
            Assert.Null(deleted);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}