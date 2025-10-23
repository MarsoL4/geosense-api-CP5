using GeoSense.API.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Implementação simples de repositório para Vaga usando MongoDB.
    /// Gera Ids usando Unix time em ms (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
    /// para evitar problemas de precisão no JavaScript/Swagger UI.
    /// </summary>
    public class VagaMongoRepository
    {
        private readonly IMongoCollection<Vaga> _collection;

        public VagaMongoRepository(IMongoClient client, MongoSettings settings)
        {
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<Vaga>("vagas");
        }

        public async Task<List<Vaga>> ObterTodasAsync()
        {
            return await _collection.Find(FilterDefinition<Vaga>.Empty).ToListAsync();
        }

        public async Task<Vaga?> ObterPorIdAsync(long id)
        {
            var filter = Builders<Vaga>.Filter.Eq(v => v.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Vaga> AdicionarAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));

            // Gera id seguro para JavaScript/Swagger UI (unix-ms)
            if (vaga.Id == 0)
            {
                vaga.Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            await _collection.InsertOneAsync(vaga);
            return vaga;
        }

        public async Task AtualizarAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));
            var filter = Builders<Vaga>.Filter.Eq(v => v.Id, vaga.Id);
            await _collection.ReplaceOneAsync(filter, vaga);
        }

        public async Task RemoverAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));
            var filter = Builders<Vaga>.Filter.Eq(v => v.Id, vaga.Id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}