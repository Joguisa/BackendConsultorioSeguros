using AutoMapper;
using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.Utility
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {

            CreateMap<TestSeguroDto, Seguro>()
            .ForMember(dest => dest.Prima, opt => opt.MapFrom(src => src.Cuota))
            // Ignora los campos no presentes en el DTO
            .ForMember(dest => dest.SeguroId, opt => opt.Ignore())
            .ForMember(dest => dest.Asegurados, opt => opt.Ignore());
        }

    }
}
