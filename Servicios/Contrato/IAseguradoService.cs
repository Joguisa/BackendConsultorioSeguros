using BackendConsultorioSeguros.DTOs;

namespace BackendConsultorioSeguros.Servicios.Contrato
{
    public interface IAseguradoService
    {
        Task<List<AseguradoDto>> GetListAsegurados();
        Task<AseguradoDto> GetAseguradoById(int aseguradoId);
        Task<CrearAseguradoDto> CreateAsegurado(CrearAseguradoDto asegurado);
        Task<CrearAseguradoDto> UpdateAsegurado(int aseguradoId, CrearAseguradoDto asegurado);
        Task<bool> InactivarAsegurado(int aseguradoId);
    }
}
