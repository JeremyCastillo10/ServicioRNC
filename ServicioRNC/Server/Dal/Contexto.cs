using Microsoft.EntityFrameworkCore;
using ServicioRNC.Server.Models;

namespace ServicioRNC.Server.Dal
{
    public class Contexto:DbContext
    {
        public Contexto(DbContextOptions <Contexto> opt):base(opt) { }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<FechaActulizacion> FechaActulizacion {  get; set; }

    }
}
