namespace GeoSense.API.Services
{
    using global::GeoSense.API.DTOs;
    using global::GeoSense.API.Infrastructure.Contexts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    namespace GeoSense.Application.Services
    {
        public class DashboardService
        {
            private readonly GeoSenseContext _context;

            public DashboardService(GeoSenseContext context)
            {
                _context = context;
            }

            public async Task<DashboardDTO> ObterDashboardAsync()
            {
                var hoje = DateTime.Today;

                // Contar quantas alocações ocorreram hoje
                var motosHoje = await _context.AlocacoesMoto
                    .Where(a => a.DataHoraAlocacao.Date == hoje)
                    .CountAsync();

                // Calcular tempo de permanência em minutos
                var tempos = await _context.AlocacoesMoto
                    .Select(a => (int)(DateTime.Now - a.DataHoraAlocacao).TotalMinutes)
                    .ToListAsync();

                // Calcular média em horas, com arredondamento
                double mediaHoras = tempos.Count > 0 ? tempos.Average() / 60.0 : 0;

                return new DashboardDTO
                {
                    TotalMotosHoje = motosHoje,
                    TempoMedioPermanenciaHoras = Math.Round(mediaHoras, 2)
                };
            }
        }
    }
}