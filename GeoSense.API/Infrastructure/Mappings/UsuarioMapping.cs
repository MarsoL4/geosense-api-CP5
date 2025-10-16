using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.Infrastructure.Mappings
{
    /// <summary>
    /// Configuração de mapeamento da entidade Usuario para o banco de dados.
    /// Define restrições de unicidade e propriedades obrigatórias.
    /// </summary>
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("USUARIO");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("ID").HasColumnType("NUMBER(19)").ValueGeneratedOnAdd();
            builder.Property(u => u.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasColumnName("EMAIL").HasMaxLength(100).IsRequired();
            builder.Property(u => u.Senha).HasColumnName("SENHA").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Tipo).HasColumnName("TIPO").HasConversion<int>().IsRequired();

            // Garante que o email do usuário seja único
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}