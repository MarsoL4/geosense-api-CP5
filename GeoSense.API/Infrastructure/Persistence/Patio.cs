namespace GeoSense.API.Infrastructure.Persistence
{
    /// <summary>
    /// Entidade que representa um pátio de alocação de motos.
    /// Cada pátio pode conter várias vagas.
    /// </summary>
    public class Patio
    {
        public long Id { get; private set; }
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Vagas disponíveis neste pátio.
        /// </summary>
        public ICollection<Vaga> Vagas { get; set; } = new List<Vaga>();
    }
}