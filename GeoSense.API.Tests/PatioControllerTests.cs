using AutoMapper;
using GeoSense.API.AutoMapper;
using GeoSense.API.Controllers;
using GeoSense.API.DTOs.Patio;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Repositories;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using GeoSense.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Tests
{
    public class PatioControllerTests
    {
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task PostPatio_DeveRetornarCreated()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio")
                .Options;

            using var context = new GeoSenseContext(options);

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var dto = new PatioDTO { Nome = "Pátio Central" };

            var result = await controller.PostPatio(dto);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task GetPatio_DeveRetornarNotFound_SeNaoExistir()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio_NotFound")
                .Options;

            using var context = new GeoSenseContext(options);

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var result = await controller.GetPatio(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutPatio_DeveRetornarNoContent_SeExistir()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio_Put")
                .Options;

            using var context = new GeoSenseContext(options);
            var patio = new Patio { Nome = "Pátio Antigo" };
            context.Patios.Add(patio);
            await context.SaveChangesAsync();

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var dto = new PatioDTO { Nome = "Pátio Novo" };
            var result = await controller.PutPatio(patio.Id, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutPatio_DeveRetornarNotFound_SeNaoExistir()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio_Put_NotFound")
                .Options;

            using var context = new GeoSenseContext(options);

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var dto = new PatioDTO { Nome = "Pátio Novo" };
            var result = await controller.PutPatio(999, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePatio_DeveRetornarNoContent_SeExistir()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio_Delete")
                .Options;

            using var context = new GeoSenseContext(options);

            var patio = new Patio { Nome = "Pátio Central" };
            context.Patios.Add(patio);
            await context.SaveChangesAsync();

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var result = await controller.DeletePatio(patio.Id);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePatio_DeveRetornarNotFound_SeNaoExistir()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Patio_Delete_NotFound")
                .Options;

            using var context = new GeoSenseContext(options);

            IPatioRepository patioRepo = new PatioRepository(context);
            var service = new PatioService(patioRepo);

            var mapper = CreateMapper();
            var controller = new PatioController(service, mapper);

            var result = await controller.DeletePatio(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}