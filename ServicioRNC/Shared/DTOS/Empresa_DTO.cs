using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioRNC.Shared.DTOS
{
    public class Empresa_DTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public string Actividad { get; set; } = string.Empty;
        public string RNC { get; set; } = string.Empty;
        public string TipoRegistro { get; set; } = string.Empty;
        public DateTime? Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
