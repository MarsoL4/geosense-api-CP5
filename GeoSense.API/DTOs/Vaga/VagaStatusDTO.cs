namespace GeoSense.API.DTOs.Vaga
{
    /// <summary>
    /// Dados agregados sobre o status das vagas livres.
    /// </summary>
    public class VagasStatusDTO
    {
        /// <summary>
        /// Quantidade de vagas livres com problema.
        /// </summary>
        public int LivresComProblema { get; set; }

        /// <summary>
        /// Quantidade de vagas livres sem problema.
        /// </summary>
        public int LivresSemProblema { get; set; }
    }
}