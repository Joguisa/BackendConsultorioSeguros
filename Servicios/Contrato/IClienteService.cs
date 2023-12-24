using BackendConsultorioSeguros.DTOs;

namespace BackendConsultorioSeguros.Servicios.Contrato
{
    public interface IClienteService
    {
        Task<List<ClienteDto>> GetListClientes();
        Task<ClienteDto> GetClienteById(int clienteId);
        Task<ClienteDto> GetClienteByCedula(string cedula);
        Task<ClienteDto> CreateCliente(ClienteDto modelo);
        Task<bool> UpdateCliente(int clienteId, ClienteDto modelo);
        Task<bool> InactivarCliente(int clienteId);
    }
}
