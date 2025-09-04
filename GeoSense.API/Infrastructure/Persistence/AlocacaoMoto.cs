namespace GeoSense.API.Infrastructure.Persistence
{
    public class AlocacaoMoto
    {
        public long Id { get; set; }
        public DateTime DataHoraAlocacao { get; set; }
        public long MotoId { get; set; }
        public long VagaId { get; set; }
        public long MecanicoResponsavelId { get; set; }
        public Moto Moto { get; set; }
        public Vaga Vaga { get; set; }
    }

}
