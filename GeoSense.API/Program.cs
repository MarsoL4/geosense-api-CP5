using Microsoft.EntityFrameworkCore;
using GeoSense.API.src.Infrastructure.Repositories;
using GeoSense.API.src.Domain.Repositories.Interfaces;
using GeoSense.API.src.Application.Services;
using GeoSense.API.src.Infrastructure.Contexts;
using GeoSense.API.src.Api.AutoMapper;

namespace GeoSense.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Repositórios do domínio
            builder.Services.AddScoped<IMotoRepository, MotoRepository>();

            // Serviços de aplicação
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<VagaService>();
            builder.Services.AddScoped<DashboardService>();

            // AutoMapper profile
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var connectionString = builder.Configuration.GetConnectionString("Oracle");

            // Registra o DbContext com a conexão Oracle
            builder.Services.AddDbContext<GeoSenseContext>(options =>
                options.UseOracle(connectionString));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}