using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.Infrastructure.Mappings;

namespace GeoSense.API.Infrastructure.Contexts
{
    public class GeoSenseContext(DbContextOptions<GeoSenseContext> options) : DbContext(options)
    {
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping());
            modelBuilder.ApplyConfiguration(new PatioMapping());
            modelBuilder.ApplyConfiguration(new VagaMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
        }
    }
}