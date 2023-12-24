using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Exceptions;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Servicios.Contrato;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendConsultorioSeguros.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            this._clienteService = clienteService;
        }

        [HttpGet(Name = "Listarclientes")]
        public async Task<ActionResult> ListarClientes()
        {
            try
            {
                var listaClientes = await _clienteService.GetListClientes();
                if (listaClientes == null)
                {
                    return NotFound(new ServiceResponse<List<ClienteDto>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }
                return Ok(new ServiceResponse<List<ClienteDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = listaClientes,
                    Message = "Proceso realizado exitosamente"
                });

            } catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<List<ClienteDto>>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<List<ClienteDto>>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{clienteId}", Name = "ObtenerCliente")]
        public async Task<ActionResult> ObtenerCliente(int clienteId)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(clienteId);
                if (cliente == null)
                {
                    return NotFound(new ServiceResponse<ClienteDto>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }
                return Ok(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = cliente,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        // POST api/<ClientesController>
        [HttpPost(Name = "crearCliente")]
        public async Task<ActionResult> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            try
            {
                var clienteCreado = await _clienteService.CreateCliente(clienteDto);
                return Ok(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = clienteCreado,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{clienteId}", Name = "actualizarcliente")]
        public async Task<ActionResult> PutCliente(int clienteId, [FromBody] ClienteDto clienteDto)
        {
            try
            {
                await _clienteService.UpdateCliente(clienteId, clienteDto);
                return Ok(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = clienteDto,
                    Message = "Cliente actualizado con éxito"
                });

            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<ClienteDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{seguroId}", Name = "DeleteCliente")]
        public async Task<ActionResult> DeleteCliente(int seguroId)
        {
            try
            {
                var clienteInactivado = await _clienteService.InactivarCliente(seguroId);
                return Ok(new ServiceResponse<bool>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = clienteInactivado,
                    Message = "Cliente eliminado con éxito"
                });

            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<bool>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = false,
                    Message = ex.Message
                });
            }
        }
    }
}
