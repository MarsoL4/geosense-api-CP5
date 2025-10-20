using GeoSense.API.Domain.Repositories;
using System.Threading.Tasks;

namespace GeoSense.API.Services
{
    public class DashboardService
    {
        private readonly IMotoRepository _motoRepo;
        private readonly IVagaRepository _vagaRepo;

        public DashboardService(IMotoRepository motoRepo, IVagaRepository vagaRepo)
        {
            _motoRepo = motoRepo;
            _vagaRepo = vagaRepo;
        }

        public async Task<object> ObterDashboardDataAsync()
        {
            var motos = await _motoRepo.ObterTodasAsync();
            var vagas = await _vagaRepo.ObterTodasAsync();

            var totalMotos = motos.Count;
            var motosComProblema = motos.Count(m => !string.IsNullOrEmpty(m.ProblemaIdentificado));

            var vagasOcupadas = vagas.Count(v => v.Motos.Count > 0);
            var vagasLivres = vagas.Count(v => v.Motos.Count == 0);
            var totalVagas = vagas.Count;

            return new
            {
                TotalMotos = totalMotos,
                MotosComProblema = motosComProblema,
                VagasLivres = vagasLivres,
                VagasOcupadas = vagasOcupadas,
                TotalVagas = totalVagas
            };
        }
    }
}