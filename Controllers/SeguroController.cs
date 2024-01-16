using BackendConsultorioSeguros.DTOs;
using BackendConsultorioSeguros.Exceptions;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using BackendConsultorioSeguros.Servicios.Contrato;
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

        [HttpPut("{seguroId}", Name = "actualizarSeguro")]
        public async Task<ActionResult> UpdateSeguro(int seguroId, [FromBody] SeguroDto seguro)
        {
            try
            {
                var seguroActualizado = await _seguroService.UpdateSeguro(seguroId, seguro);

                return Ok(new ServiceResponse<SeguroDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = seguroActualizado,
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

        [HttpDelete("{seguroId}", Name = "inactivarSeguro")]
        public async Task<ActionResult> InactivarSeguro(int seguroId)
        {
            try
            {
                var seguroInactivado = await _seguroService.InactivarSeguro(seguroId);

                return Ok(new ServiceResponse<bool>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = seguroInactivado,
                    Message = "Seguro eliminado con éxito"
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
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("TestImportSegurosAsync", Name = "testseguro")]
        public async Task<IActionResult> TestImportSegurosAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Archivo no proporcionado o vacío.");
            }

            string resultado;
            using var stream = file.OpenReadStream();
            if (file.FileName.EndsWith(".txt"))
            {
                resultado = await _seguroService.ImportarDesdeTxtAsync(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                resultado = await _seguroService.ImportarDesdeExcelAsync(stream);
            }
            else
            {
                return BadRequest("Formato de archivo no soportado.");
            }

            if (resultado.StartsWith("Error"))
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        [HttpPost("TestImportADO")]
        public async Task<IActionResult> TestImportADO(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Archivo no proporcionado o vacío.");
            }

            string resultado;
            using var stream = file.OpenReadStream();
            if (file.FileName.EndsWith(".txt"))
            {
                resultado = await _seguroService.ADOImportarDesdeTxtAsync(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                resultado = await _seguroService.ADOImportarDesdeExcelAsync(stream);
            }
            else
            {
                return BadRequest("Formato de archivo no soportado.");
            }

            return resultado.StartsWith("Error") ? BadRequest(resultado) : Ok(resultado);
        }

    }

}
