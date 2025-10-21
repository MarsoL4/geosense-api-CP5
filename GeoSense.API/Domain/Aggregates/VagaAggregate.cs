using GeoSense.API.Domain.Enums;

namespace GeoSense.API.Domain.Aggregates
{
    using GeoSense.API.Infrastructure.Persistence;

    /// <summary>
    /// Aggregate root do domínio para Vaga.
    /// Encapsula regras de negócio relacionadas à alocação/liberação de motos na vaga.
    /// Não substitui a entidade de persistência — oferece conversores/adapter para sincronização.
    /// </summary>
    public sealed class VagaAggregate
    {
        public long Id { get; private set; }
        public int Numero { get; private set; }
        public TipoVaga Tipo { get; private set; }
        public StatusVaga Status { get; private set; }
        public long PatioId { get; private set; }

        // Moto alocada (id) — null quando vaga livre
        public long? MotoId { get; private set; }

        // Construtor para uso interno / construção do agregado
        private VagaAggregate(long id, int numero, TipoVaga tipo, StatusVaga status, long patioId, long? motoId)
        {
            Id = id;
            Numero = numero;
            Tipo = tipo;
            Status = status;
            PatioId = patioId;
            MotoId = motoId;
        }

        /// <summary>
        /// Cria um agregado a partir da entidade de persistência.
        /// </summary>
        public static VagaAggregate FromPersistence(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));

            // Se houver motos (histórico), pega a primeira alocada para representar a atual (coerente com model atual)
            var motoId = vaga.Motos?.FirstOrDefault()?.Id;
            var status = vaga.Status;
            return new VagaAggregate(vaga.Id, vaga.Numero, vaga.Tipo, status, vaga.PatioId, motoId);
        }

        /// <summary>
        /// Converte o agregado de volta para a entidade de persistência (atualiza campos mutáveis).
        /// Use este método ao persistir/atualizar via repositório.
        /// </summary>
        public void ApplyToPersistence(Vaga vaga)
        {
            if (vaga == null) throw new ArgumentNullException(nameof(vaga));

            vaga.Id = Id;
            vaga.Numero = Numero;
            vaga.Tipo = Tipo;
            vaga.Status = Status;
            vaga.PatioId = PatioId;

            // Garantir que a coleção de motos esteja inicializada antes de manipular
            if (vaga.Motos == null)
            {
                vaga.Motos = new List<Moto>();
            }

            // sincronização simplificada de Motos: se houver moto alocada, garante que exista na coleção
            if (MotoId.HasValue)
            {
                // Se a coleção de motos não contiver a moto, adiciona um placeholder (somente ID)
                var existing = vaga.Motos.FirstOrDefault(m => m.Id == MotoId.Value);
                if (existing == null)
                {
                    // Criamos uma moto placeholder com apenas o id para representar a alocação.
                    var placeholder = new Moto
                    {
                        Id = MotoId.Value,
                        Modelo = string.Empty,
                        Placa = string.Empty,
                        Chassi = string.Empty,
                        VagaId = vaga.Id
                    };
                    vaga.Motos.Add(placeholder);
                }
            }
            else
            {
                // Se não há MotoId, limpa a alocação atual (remove itens) — decisão simples para checkpoint
                vaga.Motos.Clear();
            }
        }

        /// <summary>
        /// Aloca uma moto nesta vaga; aplica regras do domínio (ex.: vaga deve estar livre).
        /// </summary>
        public void AlocarMoto(long motoId)
        {
            if (motoId <= 0) throw new ArgumentException("MotoId inválido.", nameof(motoId));
            if (Status == StatusVaga.OCUPADA || MotoId.HasValue)
                throw new InvalidOperationException("Vaga já está ocupada.");

            MotoId = motoId;
            Status = StatusVaga.OCUPADA;
        }

        /// <summary>
        /// Libera a vaga — remove alocação.
        /// </summary>
        public void LiberarVaga()
        {
            if (Status == StatusVaga.LIVRE && !MotoId.HasValue)
                return; // já livre

            MotoId = null;
            Status = StatusVaga.LIVRE;
        }

        /// <summary>
        /// Atualiza atributos permitidos da vaga (número, tipo, patio).
        /// Não altera alocação.
        /// </summary>
        public void AtualizarDados(int numero, TipoVaga tipo, long patioId)
        {
            if (numero <= 0) throw new ArgumentException("Número da vaga deve ser maior que zero.", nameof(numero));
            Numero = numero;
            Tipo = tipo;
            PatioId = patioId;
        }
    }
}