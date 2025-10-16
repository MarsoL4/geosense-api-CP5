using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração de mapeamento da entidade Moto para o banco de dados.
    /// Define restrições, relacionamentos, unicidade e propriedades obrigatórias.
    /// </summary>
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.ToTable("MOTO");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasColumnName("ID").HasColumnType("NUMBER(19)").ValueGeneratedOnAdd();
            builder.Property(m => m.Modelo).HasColumnName("MODELO").HasMaxLength(50).IsRequired();
            builder.Property(m => m.Placa).HasColumnName("PLACA").HasMaxLength(10).IsRequired();
            builder.Property(m => m.Chassi).HasColumnName("CHASSI").HasMaxLength(50).IsRequired();
            builder.Property(m => m.ProblemaIdentificado).HasColumnName("PROBLEMA_IDENTIFICADO").HasMaxLength(255); // Permite nulo

            builder.Property(m => m.VagaId).HasColumnName("VAGA_ID").IsRequired();

            // Relacionamento: Cada Moto está vinculada a uma Vaga, uma vaga pode ter várias motos (histórico)
            builder.HasOne(m => m.Vaga)
                .WithMany(v => v.Motos)
                .HasForeignKey(m => m.VagaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrições de unicidade para garantir que não haja placas ou chassis duplicados
            builder.HasIndex(m => m.Placa).IsUnique();
            builder.HasIndex(m => m.Chassi).IsUnique();
            // Garante que uma vaga só pode estar com uma moto alocada por vez
            builder.HasIndex(m => m.VagaId).IsUnique();
        }
    }
}