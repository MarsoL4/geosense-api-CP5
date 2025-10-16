using Xunit;
using GeoSense.API.Controllers;
using GeoSense.API.Infrastructure.Contexts;
using GeoSense.API.Infrastructure.Repositories;
using GeoSense.API.Infrastructure.Repositories.Interfaces;
using GeoSense.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GeoSense.API.Tests
{
    public class DashboardControllerTests
    {
        [Fact]
        public async Task GetDashboardData_DeveRetornarOk()
        {
            var options = new DbContextOptionsBuilder<GeoSenseContext>()
                .UseInMemoryDatabase(databaseName: "GeoSenseTestDb_Dashboard")
                .Options;

            using var context = new GeoSenseContext(options);

            IMotoRepository motoRepo = new MotoRepository(context);
            IVagaRepository vagaRepo = new VagaRepository(context);

            var service = new DashboardService(motoRepo, vagaRepo);
            var controller = new DashboardController(service);

            var result = await controller.GetDashboardData();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}