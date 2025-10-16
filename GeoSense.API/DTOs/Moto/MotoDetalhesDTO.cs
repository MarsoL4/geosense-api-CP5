namespace GeoSense.API.DTOs.Moto
{
    /// <summary>
    /// Representa os dados de detalhe de uma Moto.
    /// </summary>
    public class MotoDetalhesDTO
    {
        public long Id { get; set; }
        public required string Modelo { get; set; }
        public required string Placa { get; set; }
        public required string Chassi { get; set; }
        public string? ProblemaIdentificado { get; set; }
        public long VagaId { get; set; }
    }
}