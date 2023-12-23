using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Exceptions;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
using BackendConsultorioSeguros.Servicios.Implementacion;
using BackendConsultorioSeguros.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendConsultorioSeguros.Controllers
{
    [ApiController]
    [Route("api/seguros")]
    public class SeguroController : ControllerBase
    {

        private readonly ISeguroService _seguroService;

        public SeguroController(ISeguroService seguroService)
        {
            this._seguroService = seguroService;
        }

        [HttpGet(Name = "listaseguros")]
        public async Task<ActionResult> GetListSeguros()
        {
            try
            {
                var listaSeguros = await _seguroService.GetListSeguros();

                if (listaSeguros == null)
                {
                    return NotFound(new ServiceResponse<List<SeguroDto>>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }

                return Ok(new ServiceResponse<List<SeguroDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = listaSeguros,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<List<Seguro>>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<List<Seguro>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{seguroId}", Name = "seguroById")]
        public async Task<ActionResult> GetSeguroById(int seguroId)
        {
            try
            {
                var seguro = await _seguroService.GetSeguroById(seguroId);

                if (seguro == null)
                {
                    return NotFound(new ServiceResponse<SeguroDto>()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No se encontraron registros"
                    });
                }

                return Ok(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = seguro,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }


        [HttpPost(Name = "crearSeguro")]
        public async Task<ActionResult> CreateSeguro([FromBody] SeguroDto seguro)
        {
            try
            {
                var seguroCreado = await _seguroService.CreateSeguro(seguro);

                return Ok(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = seguroCreado,
                    Message = "Proceso realizado exitosamente"
                });
            }
            catch (Exception ex) when (ex is ServiceException)
            {
                return BadRequest(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = ex.Message
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message
                });
            }
        }




    }

}
