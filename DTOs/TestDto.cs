namespace BackendConsultorioSeguros.DTOs
{
    public class TestDto
    {
    }

    public class TestClienteDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
    }

    public class TestSeguroDto
    {
        public string NombreSeguro { get; set; } = null!;
        public string CodigoSeguro { get; set; } = null!;
        public decimal SumaAsegurada { get; set; }
        public decimal Cuota { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Estado { get; set; }
    }
}
