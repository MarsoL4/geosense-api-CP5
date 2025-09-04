using AutoMapper;
using GeoSense.API.DTOs;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.AutoMapper
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
