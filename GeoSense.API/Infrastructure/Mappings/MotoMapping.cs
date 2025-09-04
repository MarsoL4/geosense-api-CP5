using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Mappings
{
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
            builder.Property(m => m.ProblemaIdentificado).HasColumnName("PROBLEMA_IDENTIFICADO").HasMaxLength(255).IsRequired();

            builder.Property(m => m.VagaId).HasColumnName("VAGA_ID").IsRequired();

            builder.HasOne(m => m.Vaga)
                .WithMany(v => v.Motos)
                .HasForeignKey(m => m.VagaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
