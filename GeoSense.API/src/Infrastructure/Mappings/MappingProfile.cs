using AutoMapper;
using GeoSense.API.src.Application.DTOs;
using GeoSense.API.src.Domain.Entities;

namespace GeoSense.API.src.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Moto, MotoListagemDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Vaga.Status))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Vaga.Tipo));
        }
    }
}
