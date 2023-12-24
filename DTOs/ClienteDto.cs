namespace BackendConsultorioSeguros.DTOs
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Cedula { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public int Edad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Estado { get; set; }
    }
}
