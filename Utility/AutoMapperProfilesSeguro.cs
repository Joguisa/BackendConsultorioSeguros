using AutoMapper;
using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.Utility
{
    public class AutoMapperProfilesSeguro : Profile
    {
        public AutoMapperProfilesSeguro()
        {
            CreateMap<SeguroDto, Seguro>()
                .ForMember(dest => dest.Prima, opt => opt.MapFrom(src => src.Prima))
                .ForMember(dest => dest.SeguroId, opt => opt.Ignore())
                .ForMember(dest => dest.Asegurados, opt => opt.Ignore());
        }
    }
}
