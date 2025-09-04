using AutoMapper;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Entities;

namespace GeoSense.API.src.Api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Moto, MotoListagemDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Vaga.Status))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Vaga.Tipo));

            CreateMap<Moto, MotoDetalhesDTO>()
                .ForMember(dest => dest.VagaStatus, opt => opt.MapFrom(src => src.Vaga.Status))
                .ForMember(dest => dest.VagaTipo, opt => opt.MapFrom(src => src.Vaga.Tipo))
                .ForMember(dest => dest.Defeitos, opt => opt.MapFrom(src => src.Defeitos.Select(d => d.Descricao)));
        }
    }
}
