using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;

namespace BackendConsultorioSeguros.Servicios.Implementacion
{
    public class ClienteService : IClienteService
    {

        private readonly DBSEGUROSCHUBBContext _context;

        public ClienteService(DBSEGUROSCHUBBContext context)
        {
            this._context = context;
        }

        public async Task<List<ClienteDto>> GetListClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.Estado == "A")
                    .ToListAsync();
                var listaClientes = clientes.Select(c => new ClienteDto
                {
                    ClienteId = c.ClienteId,
                    Cedula = c.Cedula,
                    NombreCliente = c.NombreCliente,
                    Telefono = c.Telefono,
                    Edad = c.Edad,
                    FechaCreacion = c.FechaCreacion,
                    Estado = c.Estado

                }).ToList();

                return listaClientes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto?> GetClienteById(int clienteId)
        {
            try
            {
                Cliente? cliente = await _context.Clientes
                .Where(c => c.ClienteId == clienteId && c.Estado == "A")
                .FirstOrDefaultAsync();

                return cliente == null
                    ? null
                    : new ClienteDto
                    {
                        Cedula = cliente.Cedula,
                        NombreCliente = cliente.NombreCliente,
                        Telefono = cliente.Telefono,
                        Edad = cliente.Edad,
                        FechaCreacion = cliente.FechaCreacion,
                        Estado = cliente.Estado
                    };

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ClienteDto> CreateCliente(ClienteDto modelo)
        {
            try
            {
                if (string.IsNullOrEmpty(modelo.NombreCliente)
                    || string.IsNullOrEmpty(modelo.Cedula)
                    || string.IsNullOrEmpty(modelo.Telefono)
                    || modelo.Edad <= 0)
                {
                    throw new ArgumentException("Los campos requeridos deben ser proporcionados.");
                }

                // Verificar si ya existe un cliente con la misma cédula
                if (await ClienteConCedulaExiste(modelo.Cedula))
                {
                    throw new InvalidOperationException("Ya existe un cliente con la misma cédula.");
                }

                var cliente = new Cliente
                {
                    Cedula = modelo.Cedula,
                    NombreCliente = modelo.NombreCliente,
                    Telefono = modelo.Telefono,
                    Edad = modelo.Edad
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                modelo.ClienteId = cliente.ClienteId;

                return modelo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> ClienteConCedulaExiste(string cedula)
        {
            return await _context.Clientes.AnyAsync(c => c.Cedula == cedula);
        }

        public Task<ClienteDto> GetClienteByCedula(string cedula)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCliente(int clienteId, ClienteDto modelo)
        {


            try
            {
                if (string.IsNullOrEmpty(modelo.NombreCliente)
                    || string.IsNullOrEmpty(modelo.Cedula)
                    || string.IsNullOrEmpty(modelo.Telefono)
                    || modelo.Edad <= 0)
                {
                    throw new ArgumentException("ID y otros campos requeridos deben ser proporcionados.");
                }

                // Verificar si ya existe un cliente con la misma cédula
                if (await ClienteConCedulaExiste(modelo.Cedula))
                {
                    throw new InvalidOperationException("Ya existe un cliente con la misma cédula.");
                }

                var clienteExistente = await _context.Clientes.FindAsync(clienteId);
                if (clienteExistente == null)
                {
                    throw new ArgumentException($"No se encontró un cliente con el ID: {clienteId}");
                }

                clienteExistente.Cedula = modelo.Cedula;
                clienteExistente.NombreCliente = modelo.NombreCliente;
                clienteExistente.Telefono = modelo.Telefono;
                clienteExistente.Edad = modelo.Edad;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InactivarCliente(int clienteId)
        {
            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(clienteId);

                if (clienteExistente == null)
                {
                    return false;
                }

                clienteExistente.Estado = "I";

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
