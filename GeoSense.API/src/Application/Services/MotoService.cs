using AutoMapper;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Entities;
using GeoSense.API.src.Domain.Repositories.Interfaces;
using GeoSense.API.src.Domain.ValueObjects;

namespace GeoSense.API.src.Application.Services
{
    public class MotoService
    {
        private readonly IMotoRepository _repo;
        private readonly IMapper _mapper;

        public MotoService(IMotoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<MotoListagemDTO>> ObterTodasAsync()
        {
            var motos = await _repo.ObterTodasAsync();
            return _mapper.Map<List<MotoListagemDTO>>(motos);
        }

        public async Task<MotoDetalhesDTO?> ObterPorIdAsync(long id)
        {
            var moto = await _repo.ObterPorIdComVagaEDefeitosAsync(id);
            if (moto == null) return null;

            return new MotoDetalhesDTO
            {
                Id = moto.Id,
                Modelo = moto.Modelo,
                Placa = moto.Placa?.ToString() ?? string.Empty,
                Chassi = moto.Chassi,
                ProblemaIdentificado = moto.ProblemaIdentificado,
                VagaStatus = moto.Vaga?.Status.ToString(),
                VagaTipo = moto.Vaga?.Tipo.ToString(),
                Defeitos = moto.Defeitos?.Select(d => d.Descricao).ToList() ?? new List<string>()
            };
        }

        public async Task<MotoDetalhesDTO?> CriarAsync(MotoDTO dto)
        {
            var placa = new Placa(dto.Placa);

            var novaMoto = new Moto
            {
                Modelo = dto.Modelo,
                Placa = placa,
                Chassi = dto.Chassi,
                VagaId = dto.VagaId
            };
            // Use o método de domínio para registrar problema, já que o setter é privado
            if (!string.IsNullOrWhiteSpace(dto.ProblemaIdentificado))
                novaMoto.RegistrarProblema(dto.ProblemaIdentificado);

            await _repo.AdicionarAsync(novaMoto);

            return await ObterPorIdAsync(novaMoto.Id);
        }

        public async Task<bool> AtualizarAsync(long id, MotoDTO dto)
        {
            var moto = await _repo.ObterPorIdComVagaEDefeitosAsync(id);
            if (moto == null) return false;

            moto.Modelo = dto.Modelo;
            moto.Placa = new Placa(dto.Placa);
            moto.Chassi = dto.Chassi;
            moto.VagaId = dto.VagaId;
            moto.RegistrarProblema(dto.ProblemaIdentificado);

            await _repo.AtualizarAsync(moto);
            return true;
        }

        public async Task<bool> RemoverAsync(long id)
        {
            var moto = await _repo.ObterPorIdComVagaEDefeitosAsync(id);
            if (moto == null) return false;
            await _repo.RemoverAsync(moto);
            return true;
        }
    }
}