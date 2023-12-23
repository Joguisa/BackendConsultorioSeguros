namespace BackendConsultorioSeguros.DTOs
{
    public class SeguroDto
    {
        public int SeguroId { get; set; }
        public string NombreSeguro { get; set; } = null!;
        public string CodigoSeguro { get; set; } = null!;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Estado { get; set; }
    }
}
