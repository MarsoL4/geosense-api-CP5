using GeoSense.API.Infrastructure.Persistence;
using MongoDB.Driver;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Repositório Mongo para Usuario (CRUD + EmailExisteAsync).
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
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(long id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            if (usuario.Id == 0)
                usuario.Id = DateTime.UtcNow.Ticks;

            await _collection.InsertOneAsync(usuario);
            return usuario;
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            await _collection.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            await _collection.DeleteOneAsync(u => u.Id == usuario.Id);
        }

        public async Task<bool> EmailExisteAsync(string email, long? ignoreId = null)
        {
            var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email);
            if (ignoreId.HasValue)
            {
                filter = Builders<Usuario>.Filter.And(filter, Builders<Usuario>.Filter.Ne(u => u.Id, ignoreId.Value));
            }

            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}