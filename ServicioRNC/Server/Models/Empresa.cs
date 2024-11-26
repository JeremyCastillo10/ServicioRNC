namespace ServicioRNC.Server.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string ?Nombre { get; set; }
        public string ?NombreComercial { get; set; }
        public string ?Actividad { get; set; }
        public string ?RNC { get; set; }
        public string? TipoRegistro { get; set; }
        public DateTime? Fecha { get; set; }
        public string ?Estado { get; set; }
    }
}
