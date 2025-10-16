namespace GeoSense.API.DTOs.Vaga
{
    /// <summary>
    /// Representa os dados necessários para cadastrar ou atualizar uma Vaga.
    /// </summary>
    public class VagaDTO
    {
        /// <summary>
        /// Identificador único da vaga (gerado pelo banco).
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Número da vaga.
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Tipo da vaga (ver enum TipoVaga).
        /// </summary>
        public int Tipo { get; set; }

        /// <summary>
        /// Status da vaga (ver enum StatusVaga).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Id do pátio associado.
        /// </summary>
        public long PatioId { get; set; }

        /// <summary>
        /// Id da moto alocada na vaga, se houver.
        /// </summary>
        public long? MotoId { get; set; }
    }
}