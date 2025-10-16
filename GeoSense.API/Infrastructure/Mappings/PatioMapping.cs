using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração de mapeamento da entidade Patio para o banco de dados.
    /// Relaciona pátio com suas vagas e define restrições de unicidade.
    /// </summary>
    public class PatioMapping : IEntityTypeConfiguration<Patio>
    {
        public void Configure(EntityTypeBuilder<Patio> builder)
        {
            builder.ToTable("PATIO");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("ID").HasColumnType("NUMBER(19)").ValueGeneratedOnAdd();

            builder.Property(p => p.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            // Um pátio pode ter várias vagas, relacionamento está definido na entidade VagaMapping.
        }
    }
}