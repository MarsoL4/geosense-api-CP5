using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Health check customizado para validar conectividade com MongoDB.
    /// </summary>
    public class MongoHealthCheck(IMongoClient client, IConfiguration configuration) : IHealthCheck
    {
        private readonly IMongoClient _client = client;
        private readonly string _databaseName = configuration.GetValue<string>("MongoSettings:DatabaseName") ?? "geosense";

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = _client.GetDatabase(_databaseName);
                var command = new BsonDocument("ping", 1);
                await database.RunCommandAsync<BsonDocument>(command, cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy("MongoDB disponível");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha ao conectar no MongoDB", ex);
            }
        }
    }
}