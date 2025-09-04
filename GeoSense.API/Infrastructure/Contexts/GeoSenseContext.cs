using GeoSense.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.Infrastructure.Mappings;

namespace GeoSense.API.Infrastructure.Contexts
{
    public class GeoSenseContext : DbContext
    {
        public GeoSenseContext(DbContextOptions<GeoSenseContext> options)
            : base(options)
        {
        }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Defeito> Defeitos { get; set; }
        public DbSet<AlocacaoMoto> AlocacoesMoto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping());
            modelBuilder.ApplyConfiguration(new PatioMapping());
            modelBuilder.ApplyConfiguration(new VagaMapping());
            modelBuilder.ApplyConfiguration(new DefeitoMapping());
            modelBuilder.ApplyConfiguration(new AlocacaoMotoMapping());
        }
    }
}