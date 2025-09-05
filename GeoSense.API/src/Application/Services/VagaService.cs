using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Entities;
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

        public async Task<List<VagaDTO>> ObterTodasAsync()
        {
            return await _context.Vagas
                .Select(v => new VagaDTO
                {
                    Numero = v.Numero,
                    Tipo = (int)v.Tipo,
                    Status = (int)v.Status,
                    PatioId = v.PatioId
                })
                .ToListAsync();
        }

        public async Task<VagaDTO?> ObterPorIdAsync(long id)
        {
            var v = await _context.Vagas.FindAsync(id);
            if (v == null) return null;

            return new VagaDTO
            {
                Numero = v.Numero,
                Tipo = (int)v.Tipo,
                Status = (int)v.Status,
                PatioId = v.PatioId
            };
        }

        public async Task<long?> CriarAsync(VagaDTO dto)
        {
            var vaga = new Vaga(dto.Numero, dto.PatioId);
            vaga.AlterarTipo((TipoVaga)dto.Tipo);

            if ((StatusVaga)dto.Status == StatusVaga.OCUPADA)
                vaga.Ocupar();
            else
                vaga.Liberar();

            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();
            return vaga.Id;
        }

        public async Task<bool> AtualizarAsync(long id, VagaDTO dto)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null) return false;

            if (vaga.Numero != dto.Numero)
                vaga.AlterarNumero(dto.Numero);

            vaga.AlterarTipo((TipoVaga)dto.Tipo);

            if ((StatusVaga)dto.Status == StatusVaga.OCUPADA)
                vaga.Ocupar();
            else
                vaga.Liberar();

            if (vaga.PatioId != dto.PatioId)
                vaga.AlterarPatio(dto.PatioId);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverAsync(long id)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null) return false;
            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}