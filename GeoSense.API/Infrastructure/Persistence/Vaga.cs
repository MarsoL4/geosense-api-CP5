using GeoSense.API.Domain;
using GeoSense.API.Domain.Enums;

namespace GeoSense.API.Infrastructure.Persistence
{
    /// <summary>
    /// Entidade que representa uma vaga disponível em um pátio.
    /// Cada vaga pode ter uma moto alocada e pertence a um pátio.
    /// </summary>
    public class Vaga
    {
        public long Id { get; private set; }
        public int Numero { get; private set; }
        public TipoVaga Tipo { get; private set; }
        public StatusVaga Status { get; private set; }
        public long PatioId { get; private set; }

        /// <summary>
        /// Pátio ao qual essa vaga pertence.
        /// </summary>
        public virtual Patio? Patio { get; set; } // nullable para evitar warning

        protected Vaga() { }

        public Vaga(int numero, long patioId)
        {
            Numero = numero;
            Tipo = TipoVaga.Sem_Problema;
            Status = StatusVaga.LIVRE;
            PatioId = patioId;
        }

        /// <summary>
        /// Motos que estão alocadas nesta vaga.
        /// </summary>
        public ICollection<Moto> Motos { get; set; } = [];
    }
}