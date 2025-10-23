namespace GeoSense.API.Domain.Entities
{
    /// <summary>
    /// Entidade que representa um pátio de alocação de motos.
    /// Cada pátio pode conter várias vagas.
    /// </summary>
    public class Patio
    {
        // Tornado public setter para compatibilidade com operações de persistência externas (Mongo)
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Vagas disponíveis neste pátio.
        /// </summary>
        public ICollection<Vaga> Vagas { get; set; } = new List<Vaga>();
    }
}