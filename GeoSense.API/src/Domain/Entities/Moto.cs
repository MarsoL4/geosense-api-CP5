namespace GeoSense.API.src.Domain.Entities
{
    public class Moto
    {
        public long Id { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Chassi { get; set; }
        public string ProblemaIdentificado { get; set; }

        // 1..1
        public long VagaId { get; set; }
        public virtual Vaga Vaga { get; set; }

        public Moto() { }

        public Moto(long id, string modelo, string placa, string chassi, string problema_identificado, long vaga_id)
        {
            Id = id;
            Modelo = modelo;
            Placa = placa;
            Chassi = chassi;
            ProblemaIdentificado = problema_identificado;
            VagaId= vaga_id;
        }

        public ICollection<Defeito> Defeitos { get; set; } = new List<Defeito>();

        public ICollection<AlocacaoMoto> Alocacoes { get; set; } = new List<AlocacaoMoto>();


    }
}
