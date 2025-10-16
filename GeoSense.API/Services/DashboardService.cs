using GeoSense.API.Infrastructure.Repositories.Interfaces;

namespace GeoSense.API.Services
{
    /// <summary>
    /// Serviço responsável por retornar dados agregados para o dashboard.
    /// </summary>
    public class DashboardService(IMotoRepository motoRepo, IVagaRepository vagaRepo)
    {
        private readonly IMotoRepository _motoRepo = motoRepo;
        private readonly IVagaRepository _vagaRepo = vagaRepo;

        /// <summary>
        /// Retorna dados agregados para o dashboard: totais de motos, vagas e problemas.
        /// </summary>
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