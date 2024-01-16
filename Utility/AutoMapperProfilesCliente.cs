using AutoMapper;
using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.Utility
{
    public class AutoMapperProfilesCliente : Profile
    {
        public AutoMapperProfilesCliente()
        {
            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.Cedula))
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Asegurados, opt => opt.Ignore());
        }
    }
}
