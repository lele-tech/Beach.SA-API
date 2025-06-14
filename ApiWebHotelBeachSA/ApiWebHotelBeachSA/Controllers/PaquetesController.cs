using ApiWebHotelBeachSA.Models;
using ApiWebHotelBeachSA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebHotelBeachSA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaquetesController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        public PaquetesController(DbContextHotelBeachSA pContext)
        {
            _context = pContext;
        }

        [HttpGet("Listado")]
        public List<Paquete> Listado()
        {
            List<Paquete> lista = _context.Paquetes.ToList();
            return lista;
        }

        [Authorize]
        [HttpPost("Agregar")]
        public async Task<string> Agregar(Paquete temp)
        {
            string mensaje = "Debe ingresar la información del paquete";

            if (temp == null)
            {
                return mensaje;
            }
            else
            {
                try
                {
                    _context.Paquetes.Add(temp);
                    await _context.SaveChangesAsync();
                    mensaje = $"Paquete {temp.Nombre} almacenado con éxito.";
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al agregar el paquete {temp.Nombre}. Detalle: {ex.InnerException?.Message}";
                }
                return mensaje;
            }
        }

        [Authorize]
        [HttpDelete("Eliminar")]
        public async Task<string> Eliminar(int id)
        {
            string mensaje = $"Paquete no eliminado. ID {id} no existe.";

            Paquete temp = _context.Paquetes.FirstOrDefault(x => x.PaqueteId == id);

            if (temp != null)
            {
                _context.Paquetes.Remove(temp);
                await _context.SaveChangesAsync();
                mensaje = $"Paquete {temp.Nombre} eliminado correctamente.";
            }

            return mensaje;
        }

        [Authorize]
        [HttpPut("Editar")]
        public async Task<string> Editar(Paquete temp)
        {
            var aux = _context.Paquetes.FirstOrDefault(x => x.PaqueteId == temp.PaqueteId);

            string mensaje = "";

            if (aux != null)
            {
                aux.Nombre = temp.Nombre;
                aux.PlazoMeses = temp.PlazoMeses;
                aux.CostoPersona = temp.CostoPersona;
                aux.PrimaPorcentaje = temp.PrimaPorcentaje;

                _context.Paquetes.Update(aux);
                await _context.SaveChangesAsync();
                mensaje = $"Paquete {aux.Nombre} actualizado correctamente.";
            }
            else
            {
                mensaje = $"El paquete con ID {temp.PaqueteId} no existe.";
            }

            return mensaje;
        }

        [HttpGet("Buscar")]
        public Paquete Buscar(int id)
        {
            Paquete temp = _context.Paquetes.FirstOrDefault(x => x.PaqueteId == id);
            return temp == null ? new Paquete() { Nombre = "No existe" } : temp;
        }
    }
}
