using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using GeoSense.API.Domain.Enums;

namespace GeoSense.API.Infrastructure.Persistence
{
    /// <summary>
    /// Entidade que representa uma vaga disponível em um pátio.
    /// Cada vaga pode ter uma moto alocada e pertence a um pátio.
    /// </summary>
    public class Vaga
    {
        // Mapeia a propriedade Id para o _id do MongoDB (Int64).
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long Id { get; set; }

        public int Numero { get; set; }
        public TipoVaga Tipo { get; set; }
        public StatusVaga Status { get; set; }
        public long PatioId { get; set; }

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
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();
    }
}