using GeoSense.API.src.Domain.Enums;

namespace GeoSense.API.src.Domain.Entities
{
    public class Vaga
    {
        public long Id { get; private set; }
        public int Numero { get; private set; }
        public TipoVaga Tipo { get; private set; }
        public StatusVaga Status { get; private set; }
        public long PatioId { get; private set; }
        public virtual Patio Patio { get; set; }

        public ICollection<Moto> Motos { get; set; } = new List<Moto>();
        public ICollection<AlocacaoMoto> Alocacoes { get; set; } = new List<AlocacaoMoto>();

        protected Vaga() { }

        public Vaga(int numero, long patioId)
        {
            Numero = numero;
            Tipo = TipoVaga.Sem_Problema;
            Status = StatusVaga.LIVRE;
            PatioId = patioId;
        }

        public void Ocupar()
        {
            if (Status == StatusVaga.OCUPADA)
                throw new InvalidOperationException("A vaga já está ocupada.");
            Status = StatusVaga.OCUPADA;
        }

        public void Liberar()
        {
            if (Status == StatusVaga.LIVRE)
                throw new InvalidOperationException("A vaga já está livre.");
            Status = StatusVaga.LIVRE;
        }

        public void AlterarTipo(TipoVaga novoTipo)
        {
            Tipo = novoTipo;
        }
    }
}