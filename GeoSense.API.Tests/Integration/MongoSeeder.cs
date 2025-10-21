using MongoDB.Driver;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Seeder opcional para popular coleções Mongo com dados de exemplo.
    /// Use manualmente a partir de um runner/console ou invoque de Program quando necessário (opcional).
    /// </summary>
    public static class MongoSeeder
    {
        public static async Task SeedAsync(IMongoClient client, MongoSettings settings, CancellationToken cancellationToken = default)
        {
            var db = client.GetDatabase(settings.DatabaseName);

            var vagas = db.GetCollection<Vaga>("vagas");
            var motos = db.GetCollection<Moto>("motos");
            var usuarios = db.GetCollection<Usuario>("usuarios");

            // Se já existe conteúdo, não re-seed
            var vagaCount = await vagas.CountDocumentsAsync(FilterDefinition<Vaga>.Empty, cancellationToken: cancellationToken);
            if (vagaCount > 0) return;

            // Seed example patios/vagas/motos/usuarios
            var patio = new Patio { Id = DateTime.UtcNow.Ticks % 1000000, Nome = "Pátio Seed" };
            var vaga1 = new Vaga(1, patio.Id) { Id = DateTime.UtcNow.Ticks % 10000000 };
            var vaga2 = new Vaga(2, patio.Id) { Id = (DateTime.UtcNow.Ticks % 10000000) + 1 };

            patio.Vagas.Add(vaga1);
            patio.Vagas.Add(vaga2);

            // Insert vagas
            await vagas.InsertManyAsync(new[] { vaga1, vaga2 }, cancellationToken: cancellationToken);

            // Insert a sample moto in vaga1
            var moto = new Moto
            {
                Id = DateTime.UtcNow.Ticks % 100000000,
                Modelo = "Honda Seed",
                Placa = "SEED0001",
                Chassi = "SEEDCHASSI0001",
                VagaId = vaga1.Id
            };
            await motos.InsertOneAsync(moto, cancellationToken: cancellationToken);

            // Insert sample usuario
            var usuario = new Usuario(0, "Seed User", "seed.user@example.com", "senha", Domain.Enums.TipoUsuario.ADMINISTRADOR);
            usuario.Id = DateTime.UtcNow.Ticks % 100000000;
            await usuarios.InsertOneAsync(usuario, cancellationToken: cancellationToken);
        }
    }
}