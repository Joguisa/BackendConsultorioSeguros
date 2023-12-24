using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Exceptions;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendConsultorioSeguros.Controllers
{
    [Route("api/asegurados")]
    [ApiController]
    public class AseguradoController : ControllerBase
    {
        private readonly IAseguradoService _aseguradoService;

        public AseguradoController(IAseguradoService aseguradoService)
        {
            this._aseguradoService = aseguradoService;
        }

        [HttpGet(Name = "listaasegurados")]
        public async Task<ActionResult> GetListAsegurados()
        {
            try
            {
                var listaAsegurados = await _aseguradoService.GetListAsegurados();

                if (listaAsegurados == null)
                {
                    return NotFound(new ServiceResponse<List<AseguradoDto>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }

                return Ok(new ServiceResponse<List<AseguradoDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = listaAsegurados,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<List<AseguradoDto>>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<List<Asegurado>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }


        [HttpGet("{aseguradoId}", Name = "aseguradobyid")]
        public async Task<ActionResult> GetAseguradoById(int aseguradoId)
        {
            try
            {
                var asegurado = await _aseguradoService.GetAseguradoById(aseguradoId);

                if (asegurado == null)
                {
                    return NotFound(new ServiceResponse<AseguradoDto>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }

                return Ok(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = asegurado,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpPost(Name = "crearasegurado")]
        public async Task<ActionResult> CreateAsegurado([FromBody] CrearAseguradoDto asegurado)
        {
            try
            {
                var aseguradoCreado = await _aseguradoService.CreateAsegurado(asegurado);

                return Ok(new ServiceResponse<CrearAseguradoDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = aseguradoCreado,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{aseguradoId}", Name = "actualizarasegurado")]
        public async Task<ActionResult> UpdateAsegurado(int aseguradoId, [FromBody] CrearAseguradoDto asegurado)
        {
            try
            {
                var aseguradoActualizado = await _aseguradoService.UpdateAsegurado(aseguradoId, asegurado);

                return Ok(new ServiceResponse<CrearAseguradoDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = aseguradoActualizado,
                    Message = "Asegurado actualizado con éxito"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{aseguradoId}", Name = "inactivarasegurado")]
        public async Task<ActionResult> InactivarAsegurado(int aseguradoId)
        {
            try
            {
                var aseguradoInactivado = await _aseguradoService.InactivarAsegurado(aseguradoId);

                return Ok(new ServiceResponse<bool>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = aseguradoInactivado,
                    Message = "Asegurado inactivado con éxito"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<AseguradoDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }
    
    }
}
