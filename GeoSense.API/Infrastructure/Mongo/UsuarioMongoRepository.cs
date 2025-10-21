using GeoSense.API.Infrastructure.Persistence;
using MongoDB.Driver;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Repositório Mongo para Usuario (CRUD + EmailExisteAsync).
    /// Gera Ids usando Unix time em ms (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).
    /// </summary>
    public class UsuarioMongoRepository
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioMongoRepository(IMongoClient client, MongoSettings settings)
        {
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<Usuario>("usuarios");
        }

        public async Task<List<Usuario>> ObterTodasAsync()
        {
            return await _collection.Find(FilterDefinition<Usuario>.Empty).ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(long id)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            if (usuario.Id == 0)
                usuario.Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await _collection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);
            await _collection.ReplaceOneAsync(filter, usuario);
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            var filter = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<bool> EmailExisteAsync(string email, long? ignoreId = null)
        {
            var builder = Builders<Usuario>.Filter;
            var filter = builder.Eq(u => u.Email, email);
            if (ignoreId.HasValue)
            {
                filter = builder.And(filter, builder.Ne(u => u.Id, ignoreId.Value));
            }

            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}