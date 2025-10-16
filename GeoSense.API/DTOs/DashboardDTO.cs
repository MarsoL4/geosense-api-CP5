namespace GeoSense.API.DTOs
{
    /// <summary>
    /// Dados agregados para o dashboard da aplicação.
    /// </summary>
    public class DashboardDTO
    {
        /// <summary>
        /// Total de motos alocadas hoje.
        /// </summary>
        public int TotalMotosHoje { get; set; }

        /// <summary>
        /// Tempo médio de permanência das motos em horas.
        /// </summary>
        public double TempoMedioPermanenciaHoras { get; set; }
    }
}