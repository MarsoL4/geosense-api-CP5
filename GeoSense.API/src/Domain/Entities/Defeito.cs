namespace GeoSense.API.src.Domain.Entities
{
    public class Defeito
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string TiposDefeitos { get; set; }
        public long MotoId { get; set; }
        public Moto Moto { get; set; }
    }
}
