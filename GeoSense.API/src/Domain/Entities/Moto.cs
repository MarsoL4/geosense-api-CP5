using GeoSense.API.src.Domain.ValueObjects;

namespace GeoSense.API.src.Domain.Entities
{
    public class Moto
    {
        public long Id { get; set; }
        public string Modelo { get; set; }
        public Placa Placa { get; set; }
        public string Chassi { get; set; }
        public string ProblemaIdentificado { get; private set; }

        public long VagaId { get; set; }
        public virtual Vaga Vaga { get; set; }

        public ICollection<Defeito> Defeitos { get; set; } = new List<Defeito>();
        public ICollection<AlocacaoMoto> Alocacoes { get; set; } = new List<AlocacaoMoto>();

        public Moto() { }

        public Moto(long id, string modelo, Placa placa, string chassi, string problemaIdentificado, long vagaId)
        {
            Id = id;
            Modelo = modelo;
            Placa = placa ?? throw new ArgumentNullException(nameof(placa));
            Chassi = chassi;
            ProblemaIdentificado = problemaIdentificado;
            VagaId = vagaId;
        }

        public void RegistrarProblema(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do problema não pode ser vazia.");
            ProblemaIdentificado = descricao;
        }

        public void AdicionarDefeito(string descricao, string tiposDefeitos)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do defeito não pode ser vazia.");

            Defeitos.Add(new Defeito
            {
                Descricao = descricao,
                TiposDefeitos = tiposDefeitos,
                MotoId = this.Id
            });
        }
    }
}