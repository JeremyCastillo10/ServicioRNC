using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicioRNC.Server.Dal;
using ServicioRNC.Server.Models;
using ServicioRNC.Shared.DTOS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ServicioRNC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DGIIController : ControllerBase
    {
        private readonly Contexto _context;

        public DGIIController(Contexto context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha seleccionado un archivo.");
            }

            var empresas = new List<Empresa>();
            try
            {
                // Cargar todos los RNCs existentes en memoria
                var existingRncs = _context.Empresas
                    .AsNoTracking()
                    .Select(e => e.RNC)
                    .ToHashSet();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    string line;
                    int index = 0;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        index++;
                        var columns = line.Split('|');
                        if (columns.Length < 11)
                        {
                            Console.WriteLine($"Formato incorrecto en la línea {index}. Se esperaban al menos 11 columnas. Línea: {line}");
                            continue;
                        }

                        var rnc = columns[0]?.Trim();  // Asegúrate de que no sea nulo ni vacío
                        if (string.IsNullOrEmpty(rnc) || existingRncs.Contains(rnc))
                        {
                            Console.WriteLine($"RNC nulo o empresa con RNC {rnc} ya existe. Omitiendo la línea {index}.");
                            continue;
                        }

                        empresas.Add(new Empresa
                        {
                            RNC = rnc,
                            Nombre = string.IsNullOrWhiteSpace(columns[1]) ? null : columns[1],
                            NombreComercial = string.IsNullOrWhiteSpace(columns[2]) ? null : columns[2],
                            Actividad = string.IsNullOrWhiteSpace(columns[3]) ? null : columns[3],
                            Fecha = DateTime.TryParse(columns[8], out var parsedDate) ? parsedDate : null,
                            Estado = string.IsNullOrWhiteSpace(columns[9]) ? null : columns[9],
                            TipoRegistro = string.IsNullOrWhiteSpace(columns[10]) ? null : columns[10]
                        });
                    }
                }

                // Insertar todo el lote en una sola operación
                if (empresas.Count > 0)
                {
                    await _context.Empresas.AddRangeAsync(empresas);
                    await _context.SaveChangesAsync();
                }

                var empresaDtos = empresas.Select(empresa => new Empresa_DTO
                {
                    Id = empresa.Id,
                    Nombre = empresa.Nombre,
                    RNC = empresa.RNC,
                    Estado = empresa.Estado,
                    Actividad = empresa.Actividad,
                    Fecha = empresa.Fecha,
                    NombreComercial = empresa.NombreComercial,
                    TipoRegistro = empresa.TipoRegistro
                }).ToList();

                return Ok(empresaDtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al procesar el archivo: {ex.Message}");
            }
        }

    }
}
