using BackendConsultorioSeguros.Exceptions;
using BackendConsultorioSeguros.Helpers;
using BackendConsultorioSeguros.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackendConsultorioSeguros.Utility
{
    public class ExceptionHandler
    {
        public static IActionResult HandleException(Exception ex)
        {
            if (ex is ServiceException)
            {
                return HandleServiceException(ex as ServiceException, HttpStatusCode.BadRequest);
            }
            return HandleInternalServerError(ex);
        }

        private static IActionResult HandleServiceException(ServiceException ex, HttpStatusCode statusCode)
        {
            return new BadRequestObjectResult(new ServiceResponse<List<Seguro>>()
            {
                StatusCode = statusCode,
                Data = null,
                Message = ex.Message
            });
        }

        private static IActionResult HandleInternalServerError(Exception ex)
        {
            return new BadRequestObjectResult(new ServiceResponse<List<Seguro>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Message = ex.Message
            });
        }
    }
}
