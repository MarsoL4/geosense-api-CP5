using GeoSense.API.AutoMapper;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Repositories;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json;
using MongoDB.Driver;
using GeoSense.API.Infrastructure.Mongo;

namespace GeoSense.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services / DI - Application services and repositories (EF)
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<IMotoRepository, MotoRepository>();

            builder.Services.AddScoped<VagaService>();
            builder.Services.AddScoped<IVagaRepository, VagaRepository>();

            builder.Services.AddScoped<PatioService>();
            builder.Services.AddScoped<IPatioRepository, PatioRepository>();

            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            builder.Services.AddScoped<DashboardService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var connectionString = builder.Configuration.GetConnectionString("Oracle");
            builder.Services.AddDbContext<GeoSenseContext>(options =>
                options.UseOracle(connectionString));

            // --- MongoDB: configuração do cliente e settings ---
            // Lê seção "MongoSettings" do appsettings.json para injeção e cria um IMongoClient singleton.
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
            var mongoSettings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>() ?? new MongoSettings
            {
                ConnectionString = builder.Configuration.GetConnectionString("Mongo") ?? "mongodb://localhost:27017",
                DatabaseName = "geosense"
            };

            // Registra a instância de MongoSettings para uso em repositórios/healthchecks
            builder.Services.AddSingleton(mongoSettings);

            // Registra o IMongoClient como singleton
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var cs = mongoSettings.ConnectionString;
                return new MongoClient(cs);
            });

            // Registra o MongoHealthCheck para ser usado pelo AddHealthChecks()
            builder.Services.AddScoped<MongoHealthCheck>();

            // Versionamento de API
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddEndpointsApiExplorer();

            // Adiciona Health Checks (EF + Mongo)
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<GeoSenseContext>("Database")
                .AddCheck<MongoHealthCheck>("MongoDB");

            // Adiciona SwaggerGen depois do VersionedApiExplorer
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeoSense API",
                    Version = "v1",
                    Description = "API RESTful para gerenciamento de Motos, Vagas, Pátios e Usuários. Endpoints CRUD, paginação, HATEOAS e exemplos de payload."
                });

                options.ExampleFilters();

                // Versionamento do Swagger
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                    var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(typeof(ApiVersionAttribute), true)
                        .Cast<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions?.Any(v => $"v{v}" == docName) ?? false;
                });
            });

            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

            var app = builder.Build();

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"GeoSense API {description.GroupName.ToUpperInvariant()}");
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Cache the JsonSerializerOptions instance to avoid performance issues (CA1869)
            var cachedJsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

            // Endpoint de health check com resposta em JSON estruturado
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            totalDuration = report.TotalDuration.ToString(),
                            entries = report.Entries.ToDictionary(
                                e => e.Key,
                                e => new
                                {
                                    status = e.Value.Status.ToString(),
                                    duration = e.Value.Duration.ToString(),
                                    tags = e.Value.Tags
                                }
                            )
                        }, cachedJsonSerializerOptions);
                    await context.Response.WriteAsync(result);
                }
            });

            app.Run();
        }
    }
}