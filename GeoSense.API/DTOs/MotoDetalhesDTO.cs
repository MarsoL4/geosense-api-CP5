namespace GeoSense.API.DTOs
{
    public class MotoDetalhesDTO
    {
        public long Id { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Chassi { get; set; }
        public string ProblemaIdentificado { get; set; }
        public string VagaStatus { get; set; }
        public string VagaTipo { get; set; }
        public List<string> Defeitos { get; set; }
    }
}