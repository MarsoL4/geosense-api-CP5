namespace GeoSense.API.src.Application.DTOs
{
    public class VagaDTO
    {
        public int Numero { get; set; }             // Número da vaga (ex: 12)
        public int Tipo { get; set; }               // Tipo da vaga (ex: 0 - comum, 1 - elétrica, etc.)
        public int Status { get; set; }             // Status (ex: 0 - livre, 1 - ocupada)
        public long PatioId { get; set; }           // FK para o pátio onde a vaga está localizada
    }
}