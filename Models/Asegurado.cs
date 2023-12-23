using System;
using System.Collections.Generic;

namespace BackendConsultorioSeguros.Models
{
    public partial class Asegurado
    {
        public int AseguradoId { get; set; }
        public int ClienteId { get; set; }
        public int SeguroId { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Seguro Seguro { get; set; } = null!;
    }
}
