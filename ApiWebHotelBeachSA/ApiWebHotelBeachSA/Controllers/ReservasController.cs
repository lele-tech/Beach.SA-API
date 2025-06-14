using ApiWebHotelBeachSA.Models;
using ApiWebHotelBeachSA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebHotelBeachSA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservasController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        public ReservasController(DbContextHotelBeachSA pContext)
        {
            _context = pContext;
        }

        [HttpGet("Listado")]
        public List<Reserva> Listado()
        {
            List<Reserva> lista = _context.Reservas.ToList();
            return lista;
        }

        [Authorize]
        [HttpPost("Agregar")]
        public async Task<string> Agregar(Reserva temp)
        {
            string mensaje = "Debe ingresar la información de la reservación";

            if (temp == null)
            {
                return mensaje;
            }
            else
            {
                try
                {
                    _context.Reservas.Add(temp);
                    await _context.SaveChangesAsync();
                    mensaje = $"Reservación {temp.ReservaId} almacenada con éxito.";
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al agregar la reservación {temp.ReservaId}. Detalle: {ex.InnerException?.Message}";
                }
                return mensaje;
            }
        }

        [Authorize]
        [HttpDelete("Eliminar")]
        public async Task<string> Eliminar(int id)
        {
            string mensaje = $"Reservación no eliminada. ID {id} no existe.";

            Reserva temp = _context.Reservas.FirstOrDefault(x => x.ReservaId == id);

            if (temp != null)
            {
                _context.Reservas.Remove(temp);
                await _context.SaveChangesAsync();
                mensaje = $"Reservación {temp.ReservaId} eliminada correctamente.";
            }

            return mensaje;
        }

        [Authorize]
        [HttpPut("Editar")]
        public async Task<string> Editar(Reserva temp)
        {
            var aux = _context.Reservas.FirstOrDefault(x => x.ReservaId == temp.ReservaId);

            string mensaje = "";

            if (aux != null)
            {
                aux.CantidadNoches = temp.CantidadNoches;
                aux.MontoTotalColones = temp.MontoTotalColones;
                aux.MontoTotalDolares = temp.MontoTotalDolares;
                aux.MontoPrima = temp.MontoPrima;
                aux.Descuento = temp.Descuento;
                aux.CedCliente = temp.CedCliente;
                aux.PaqueteId = temp.PaqueteId;
                aux.FechaReserva = temp.FechaReserva;
                aux.MetodoPago = temp.MetodoPago;
                aux.NumeroCheque = temp.NumeroCheque;
                aux.Banco = temp.Banco;
                aux.Activa = temp.Activa;

                _context.Reservas.Update(aux);
                await _context.SaveChangesAsync();
                mensaje = $"Reservación {aux.ReservaId} actualizada correctamente.";
            }
            else
            {
                mensaje = $"La reservación con ID {temp.ReservaId} no existe.";
            }

            return mensaje;
        }

        [HttpGet("Buscar")]
        public Reserva Buscar(int id)
        {
            Reserva temp = _context.Reservas.FirstOrDefault(x => x.ReservaId == id);
            return temp == null ? new Reserva() { ReservaId = 0 } : temp;
        }
    }
}
