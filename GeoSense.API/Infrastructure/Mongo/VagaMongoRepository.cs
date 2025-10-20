using GeoSense.API.Infrastructure.Persistence;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Implementação simples de repositório para Vaga usando MongoDB.
    /// Observação: para geração de Ids aqui utilizamos DateTime.UtcNow.Ticks (long).
    /// Em produção você pode usar um mecanismo de sequência, ObjectId/string, ou outra estratégia.
    /// </summary>
    public class VagaMongoRepository
    {
        private readonly IMongoCollection<Vaga> _collection;

        public VagaMongoRepository(IMongoClient client, MongoSettings settings)
        {
            var db = client.GetDatabase(settings.DatabaseName);
            // Nome da coleção: "vagas"
            _collection = db.GetCollection<Vaga>("vagas");
        }

        public async Task<List<Vaga>> ObterTodasAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Vaga?> ObterPorIdAsync(long id)
        {
            return await _collection.Find(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Vaga> AdicionarAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));

            // Se Id não foi fornecido, gerar com ticks (único o suficiente para checkpoints)
            if (vaga.Id == 0)
            {
                vaga.Id = DateTime.UtcNow.Ticks;
            }

            await _collection.InsertOneAsync(vaga);
            return vaga;
        }

        public async Task AtualizarAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));
            await _collection.ReplaceOneAsync(v => v.Id == vaga.Id, vaga);
        }

        public async Task RemoverAsync(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));
            await _collection.DeleteOneAsync(v => v.Id == vaga.Id);
        }
    }
}