namespace GeoSense.API.DTOs.Patio
{
    /// <summary>
    /// Representa os dados necessários para cadastrar ou atualizar um Pátio.
    /// </summary>
    public class PatioDTO
    {
        /// <summary>
        /// Identificador único do pátio (gerado pelo banco).
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Nome do pátio.
        /// </summary>
        public required string Nome { get; set; }
    }
}