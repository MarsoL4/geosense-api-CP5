using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Repositories.Interfaces;

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

        public async Task<MotoDetalhesDTO> ObterPorIdAsync(long id)
        {
            var moto = await _repo.ObterPorIdComVagaEDefeitosAsync(id);
            if (moto == null) return null;

            return new MotoDetalhesDTO
            {
                Id = moto.Id.GetHashCode(), // Convert Guid to a long-compatible value using GetHashCode()
                Modelo = moto.Modelo,
                Placa = moto.Placa,
                Chassi = moto.Chassi,
                ProblemaIdentificado = moto.ProblemaIdentificado,
                VagaStatus = moto.Vaga?.Status.ToString(),
                VagaTipo = moto.Vaga?.Tipo.ToString(),
                Defeitos = moto.Defeitos?.Select(d => d.Descricao).ToList() ?? new List<string>()
            };
        }
    }
}
