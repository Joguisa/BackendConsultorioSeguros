using System.Net;

namespace BackendConsultorioSeguros.Helpers
{
    public class ServiceResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
