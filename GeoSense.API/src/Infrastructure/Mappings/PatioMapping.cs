using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.src.Domain.Entities;

namespace GeoSense.API.src.Infrastructure.Mappings
{
    public class PatioMapping : IEntityTypeConfiguration<Patio>
    {
        public void Configure(EntityTypeBuilder<Patio> builder)
        {
            builder.ToTable("PATIO");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("ID").HasColumnType("NUMBER(19)").ValueGeneratedOnAdd();
        }
    }
}
