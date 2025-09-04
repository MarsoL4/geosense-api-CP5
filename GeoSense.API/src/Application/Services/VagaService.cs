using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Enums;
using GeoSense.API.src.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GeoSense.API.src.Application.Services
{
    public class VagaService
    {
        private readonly GeoSenseContext _context;

        public VagaService(GeoSenseContext context)
        {
            _context = context;
        }

        public async Task<VagasStatusDTO> ObterVagasLivresAsync()
        {
            // Busca apenas vagas com Status LIVRE, e seleciona apenas os campos necessários
            var vagasLivres = await _context.Vagas
                .Where(v => v.Status == StatusVaga.LIVRE)
                .Select(v => v.Tipo)
                .ToListAsync();

            // Conta as categorias de forma simples e direta
            var livresComProblema = vagasLivres.Count(tipo => tipo != TipoVaga.Sem_Problema);
            var livresSemProblema = vagasLivres.Count(tipo => tipo == TipoVaga.Sem_Problema);

            return new VagasStatusDTO
            {
                LivresComProblema = livresComProblema,
                LivresSemProblema = livresSemProblema
            };
        }
    }
}