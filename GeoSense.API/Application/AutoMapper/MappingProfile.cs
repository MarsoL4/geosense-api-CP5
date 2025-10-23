using System.Linq;
using AutoMapper;
using GeoSense.API.Domain.Enums;
using GeoSense.API.Application.DTOs.Moto;
using GeoSense.API.Application.DTOs.Patio;
using GeoSense.API.Application.DTOs.Usuario;
using GeoSense.API.Application.DTOs.Vaga;
using GeoSense.API.Domain.Entities;

namespace GeoSense.API.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTOs
            CreateMap<Moto, MotoDetalhesDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
                .ForMember(dest => dest.Chassi, opt => opt.MapFrom(src => src.Chassi))
                .ForMember(dest => dest.ProblemaIdentificado, opt => opt.MapFrom(src => src.ProblemaIdentificado))
                .ForMember(dest => dest.VagaId, opt => opt.MapFrom(src => src.VagaId));

            CreateMap<Vaga, VagaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (int)src.Tipo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.PatioId, opt => opt.MapFrom(src => src.PatioId))
                // Mapa o Id da primeira moto (se houver) para MotoId
                .ForMember(dest => dest.MotoId, opt => opt.MapFrom(src => src.Motos.Select(m => (long?)m.Id).FirstOrDefault()));

            CreateMap<Patio, PatioDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome));

            CreateMap<Patio, PatioDetalhesDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Vagas, opt => opt.MapFrom(src => src.Vagas));

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => src.Senha))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (int)src.Tipo));

            // DTO -> Entity / Constructions
            CreateMap<UsuarioDTO, Usuario>()
                .ConstructUsing(dto => new Usuario(0, dto.Nome, dto.Email, dto.Senha, (TipoUsuario)dto.Tipo));

            CreateMap<MotoDTO, Moto>()
                .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.Modelo))
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa))
                .ForMember(dest => dest.Chassi, opt => opt.MapFrom(src => src.Chassi))
                .ForMember(dest => dest.ProblemaIdentificado, opt => opt.MapFrom(src => src.ProblemaIdentificado))
                .ForMember(dest => dest.VagaId, opt => opt.MapFrom(src => src.VagaId));

            CreateMap<PatioDTO, Patio>()
                .ConstructUsing(dto => new Patio { Nome = dto.Nome });

            CreateMap<VagaDTO, Vaga>()
                .ConstructUsing(dto => new Vaga(dto.Numero, dto.PatioId))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (TipoVaga)src.Tipo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (StatusVaga)src.Status));
        }
    }
}