using FluentValidation;
using FluentValidation.AspNetCore;
using GeoSense.API.AutoMapper;
using GeoSense.API.Domain.Repositories;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Mongo;
using GeoSense.API.Infrastructure.Repositories;
using GeoSense.API.Services;
using GeoSense.API.Validators;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json;

namespace GeoSense.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services / DI - Application services
            builder.Services.AddScoped<MotoService>();
            builder.Services.AddScoped<VagaService>();
            builder.Services.AddScoped<PatioService>();
            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<DashboardService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // DbContext (EF/Oracle)
            var connectionString = builder.Configuration.GetConnectionString("Oracle");
            builder.Services.AddDbContext<GeoSenseContext>(options =>
                options.UseOracle(connectionString));

            // MongoDB
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
            var mongoSettings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>() ?? new MongoSettings
            {
                ConnectionString = builder.Configuration.GetConnectionString("Mongo") ?? "mongodb://localhost:27017",
                DatabaseName = "geosense"
            };

            builder.Services.AddSingleton(mongoSettings);
            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoSettings.ConnectionString));

            // Infrastructure implementations (EF) registered against Domain interfaces
            builder.Services.AddScoped<IMotoRepository, MotoRepository>();
            builder.Services.AddScoped<IVagaRepository, VagaRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Patio: mantido com a interface em Infrastructure (pode migrar depois)
            builder.Services.AddScoped<IPatioRepository, PatioRepository>();

            // Repositorios Mongo (implementações concretas para v2)
            builder.Services.AddScoped<VagaMongoRepository>();
            builder.Services.AddScoped<MotoMongoRepository>();
            builder.Services.AddScoped<UsuarioMongoRepository>();

            // HealthCheck
            builder.Services.AddScoped<MongoHealthCheck>();
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<GeoSenseContext>("Database")
                .AddCheck<MongoHealthCheck>("MongoDB");

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MotoDTOValidator>();

            // API Versioning
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

            // Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeoSense API",
                    Version = "v1",
                    Description = "GeoSense API v1 - Endpoints base (EF/Oracle)"
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "GeoSense API",
                    Version = "v2",
                    Description = "GeoSense API v2 - Endpoints com integração MongoDB"
                });

                options.ExampleFilters();

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                    var versions = methodInfo.DeclaringType?
                        .GetCustomAttributes(typeof(ApiVersionAttribute), true)
                        .Cast<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions?.Any(v => $"v{v}" == docName) ?? false;
                });

                options.EnableAnnotations();
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

            var cachedJsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

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