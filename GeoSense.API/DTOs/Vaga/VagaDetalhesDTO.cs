namespace GeoSense.API.DTOs.Vaga
{
    /// <summary>
    /// Detalhes completos de uma vaga, incluindo moto alocada.
    /// </summary>
    public class VagaDetalhesDTO
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public int Tipo { get; set; }
        public int Status { get; set; }
        public long PatioId { get; set; }
        public long? MotoId { get; set; } // Moto alocada ou null
    }
}