using GeoSense.API.Domain.Entities;
using MongoDB.Driver;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Repositório Mongo para Moto (CRUD).
    /// Gera Ids usando Unix time em ms (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
    /// para evitar problemas de precisão no JavaScript/Swagger UI.
    /// </summary>
    public class MotoMongoRepository
    {
        private readonly IMongoCollection<Moto> _collection;

        public MotoMongoRepository(IMongoClient client, MongoSettings settings)
        {
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<Moto>("motos");
        }

        public async Task<List<Moto>> ObterTodasAsync()
        {
            return await _collection.Find(FilterDefinition<Moto>.Empty).ToListAsync();
        }

        public async Task<Moto?> ObterPorIdAsync(long id)
        {
            var filter = Builders<Moto>.Filter.Eq(m => m.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Moto> AdicionarAsync(Moto moto)
        {
            if (moto == null) throw new ArgumentNullException(nameof(moto));

            // Gera id seguro para JavaScript/Swagger UI (unix-ms)
            if (moto.Id == 0)
            {
                moto.Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            await _collection.InsertOneAsync(moto);
            return moto;
        }

        public async Task AtualizarAsync(Moto moto)
        {
            if (moto == null) throw new ArgumentNullException(nameof(moto));
            var filter = Builders<Moto>.Filter.Eq(m => m.Id, moto.Id);
            await _collection.ReplaceOneAsync(filter, moto);
        }

        public async Task RemoverAsync(Moto moto)
        {
            if (moto == null) throw new ArgumentNullException(nameof(moto));
            var filter = Builders<Moto>.Filter.Eq(m => m.Id, moto.Id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}