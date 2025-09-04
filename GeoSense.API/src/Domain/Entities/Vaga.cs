using GeoSense.API.Domain;
using GeoSense.API.src.Domain.Enums;

namespace GeoSense.API.src.Domain.Entities
{
    public class Vaga
    {
        public long Id { get; private set; }
        public int Numero { get; private set; }
        public TipoVaga Tipo { get; private set; }
        public StatusVaga Status { get; private set; }

        // N...1 (muitas vagas pertencem a um pátio)
        public long PatioId { get; private set; }
        public virtual Patio Patio { get; set; }

        protected Vaga() { }

        public Vaga(int numero, long patioId)
        {
            Numero = numero;
            Tipo = TipoVaga.Sem_Problema;
            Status = StatusVaga.LIVRE;
            PatioId = patioId;
        }

        public ICollection<Moto> Motos { get; set; } = new List<Moto>();
        public ICollection<AlocacaoMoto> Alocacoes { get; set; } = new List<AlocacaoMoto>();
    }
}
