namespace GeoSense.API.Infrastructure.Mongo
{
    /// <summary>
    /// Configurações mínimas para conexão com MongoDB.
    /// </summary>
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = "geosense";
    }
}