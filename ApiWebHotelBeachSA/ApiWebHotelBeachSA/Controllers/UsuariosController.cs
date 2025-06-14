using ApiWebHotelBeachSA.Data;
using ApiWebHotelBeachSA.Models;
using ApiWebHotelBeachSA.Models.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiWebHotelBeachSA.Services;

namespace ApiWebHotelBeachSA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        private readonly IAutorizacionServices _autorizacionServices;

        public UsuariosController(DbContextHotelBeachSA pContext, IAutorizacionServices autorizacionServices)
        {
            _context = pContext;
            _autorizacionServices = autorizacionServices;
        }

        [HttpGet("Listado")]
        public List<Usuario> Listado()
        {
            List<Usuario> lista = null;

            lista = _context.Usuarios.ToList();

            return lista;
        }

        [HttpPost("Agregar")]
        public async Task<string> Agregar(Usuario temp)
        {
            string mensaje = "Debe ingresar la información del usuario";

            if (temp == null)
            {
                return mensaje;
            }
            else
            {
                try
                {
                    _context.Usuarios.Add(temp);

                    await _context.SaveChangesAsync();

                    mensaje = $"Usuario {temp.NombreCompleto} almacenado con exito..";
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al agregar el usuario {temp.NombreCompleto} detalle {ex.InnerException}";
                }
                return mensaje;
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<string> Eliminar(string email)
        {
            string mensaje = $"Usuario no eliminado {email} valor no existe..";

            Usuario temp = _context.Usuarios.FirstOrDefault(x => x.Email == email);

            if (temp != null)
            {
                _context.Usuarios.Remove(temp);
                await _context.SaveChangesAsync();
                mensaje = $"Usuario {temp.NombreCompleto} eliminado correctamente...";
            }

            return mensaje;
        }

        [HttpPut("Editar")]
        public async Task<string> Editar(Usuario temp)
        {
            var aux = _context.Usuarios.FirstOrDefault(x => x.Email == temp.Email);

            string mensaje = "";

            if (aux != null)
            {
                aux.NombreCompleto = temp.NombreCompleto;
                aux.Password = temp.Password;
                aux.FechaRegistro = temp.FechaRegistro;
                aux.RolId = temp.RolId;

                _context.Usuarios.Update(aux);

                await _context.SaveChangesAsync();

                mensaje = $"usuario {aux.Email} actualizado correctamente..";
            }
            else
            {
                mensaje = $"El usuario {temp.NombreCompleto} no existe ...";
            }
            return mensaje;
        }

        [HttpGet("Buscar")]
        public Usuario Buscar(string email)
        {
            Usuario temp = null;

            temp = _context.Usuarios.FirstOrDefault(h => h.Email == email);

            return temp == null ? new Usuario() { NombreCompleto = "No existe" } : temp;

        }




        [HttpPost]
        [Route("AutenticarPW")]
        public async Task<IActionResult> AutenticarPW(string email, string password)
        {
            var temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(email)
            && x.Password.Equals(password));

            if (temp == null)
            {
                return Unauthorized(new AutorizacionResponse
                {
                    Token = "",
                    Msj = "No autorizado",
                    Resultado = false,
                    RolId = 0 // Sin rol
                });
            }
            else
            {
                var autorizado = await _autorizacionServices.DevolverToken(temp);

                if (autorizado == null)
                {
                    return Unauthorized(new AutorizacionResponse
                    {
                        Token = "",
                        Msj = "No autorizado",
                        Resultado = false,
                        RolId = 0 // Sin rol
                    });
                }
                else
                {
                    return Ok(new AutorizacionResponse
                    {
                        Token = autorizado.Token,
                        Msj = "Autenticación exitosa",
                        Resultado = true,
                        RolId = temp.RolId // Incluye el RolId del usuario
                    });
                }
            }
        }
    }
}
