using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.DTOs
{
    public class AseguradoDto
    {
        public int AseguradoId { get; set; }
        public int ClienteId { get; set; }
        public int SeguroId { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ClienteDto Cliente { get; set; }

        // Datos del seguro
        public SeguroDto Seguro { get; set; }

    }

}
