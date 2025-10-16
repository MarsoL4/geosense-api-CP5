using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração de mapeamento da entidade Vaga para o banco de dados.
    /// Relaciona vaga com seu pátio e define restrição de unicidade composta.
    /// </summary>
    public class VagaMapping : IEntityTypeConfiguration<Vaga>
    {
        public void Configure(EntityTypeBuilder<Vaga> builder)
        {
            builder.ToTable("VAGA");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id).HasColumnName("ID").HasColumnType("NUMBER(19)").ValueGeneratedOnAdd();
            builder.Property(v => v.Numero).HasColumnName("NUMERO");
            builder.Property(v => v.Tipo).HasColumnName("TIPO");
            builder.Property(v => v.Status).HasColumnName("STATUS");
            builder.Property(v => v.PatioId).HasColumnName("PATIO_ID");

            // Relacionamento: Cada vaga pertence a um pátio
            builder.HasOne(v => v.Patio)
                   .WithMany(p => p.Vagas)
                   .HasForeignKey(v => v.PatioId);

            // Restrição de unicidade composta: Numero + PatioId. Garante que não existam duas vagas com o mesmo número no mesmo pátio
            builder.HasIndex(v => new { v.Numero, v.PatioId }).IsUnique();
        }
    }
}