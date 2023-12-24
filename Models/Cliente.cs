using System;
using System.Collections.Generic;

namespace BackendConsultorioSeguros.Models
{
    public partial class Cliente
    {

        public int ClienteId { get; set; }
        public string Cedula { get; set; } = null!;
        public string NombreCliente { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public int Edad { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Estado { get; set; }

        public virtual ICollection<Asegurado> Asegurados { get; set; } = new HashSet<Asegurado>();
    }
}
