using AutoMapper;
using GeoSense.API.DTOs;
using GeoSense.API.Infrastructure.Persistence;

namespace GeoSense.API.Infrastructure.Mappings
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
