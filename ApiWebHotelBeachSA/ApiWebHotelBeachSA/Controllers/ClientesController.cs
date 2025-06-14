using ApiWebHotelBeachSA.Models;
using ApiWebHotelBeachSA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebHotelBeachSA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        public ClientesController(DbContextHotelBeachSA pContext)
        {
            _context = pContext;
        }

        [HttpGet("Listado")]
        public List<Cliente> Listado()
        {
            List<Cliente> lista = _context.Clientes.ToList();
            return lista;
        }

        [Authorize]
        [HttpPost("Agregar")]
        public async Task<string> Agregar(Cliente temp)
        {
            string mensaje = "Debe ingresar la información del cliente";

            if (temp == null)
            {
                return mensaje;
            }
            else
            {
                try
                {
                    _context.Clientes.Add(temp);
                    await _context.SaveChangesAsync();
                    mensaje = $"Cliente {temp.NombreCompleto} almacenado con éxito.";
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al agregar el cliente {temp.NombreCompleto}. Detalle: {ex.InnerException?.Message}";
                }
                return mensaje;
            }
        }

        [Authorize]
        [HttpDelete("Eliminar")]
        public async Task<String> Eliminar(int cedula)
        {
            string mensaje = $"Cliente no eliminado. Cédula {cedula} no existe.";

            Cliente temp = _context.Clientes.FirstOrDefault(x => x.Cedula == cedula);

            if (temp != null)
            {
                _context.Clientes.Remove(temp);
                await _context.SaveChangesAsync();
                mensaje = $"Cliente {temp.NombreCompleto} eliminado correctamente.";
            }

            return mensaje;
        }

        [Authorize]
        [HttpPut("Editar")]
        public async Task<string> Editar(Cliente temp)
        {
            var aux = _context.Clientes.FirstOrDefault(x => x.Cedula == temp.Cedula);

            string mensaje = "";

            if (aux != null)
            {
                aux.NombreCompleto = temp.NombreCompleto;
                aux.Telefono = temp.Telefono;
                aux.Direccion = temp.Direccion;
                aux.Email = temp.Email;
                aux.TipoCedula = temp.TipoCedula;

                _context.Clientes.Update(aux);
                await _context.SaveChangesAsync();
                mensaje = $"Cliente {aux.NombreCompleto} actualizado correctamente.";
            }
            else
            {
                mensaje = $"El cliente con cédula {temp.Cedula} no existe.";
            }

            return mensaje;
        }

        [HttpGet("Buscar")]
        public Cliente Buscar(int cedula)
        {
            Cliente temp = _context.Clientes.FirstOrDefault(x => x.Cedula == cedula);
            return temp == null ? new Cliente() { NombreCompleto = "No existe" } : temp;
        }
    }
}
