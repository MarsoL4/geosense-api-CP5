namespace GeoSense.API.DTOs.Moto
{
    /// <summary>
    /// Representa os dados necessários para cadastrar ou atualizar uma Moto.
    /// </summary>
    public class MotoDTO
    {
        public required string Modelo { get; set; }
        public required string Placa { get; set; }
        public required string Chassi { get; set; }
        public string? ProblemaIdentificado { get; set; }
        public long VagaId { get; set; }
    }
}