using Microsoft.AspNetCore.Mvc;
using Beach.SA.Models;
using Beach.SA.Data;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Beach.SA.Models.Custom;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
 
namespace AppWebBibliotecaConsumo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly BeachApi beachAPI;
        private readonly HttpClient httpClient;

        public UsuariosController()
        {
            beachAPI = new BeachApi();
            httpClient = beachAPI.Inicial();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearCuenta()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCuenta(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor, verifica los datos ingresados.";
                return View(usuario);
            }

            // Convertimos el usuario a JSON para enviarlo a la API
            var jsonContent = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Realizamos la petición POST para crear el usuario en la API
                HttpResponseMessage response = await httpClient.PostAsync("/Usuarios/Agregar", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Cuenta creada exitosamente. Ahora puedes iniciar sesión.";
                    return RedirectToAction("Login");
                }
                else
                {
                    // Leer el contenido de error de la respuesta
                    var statusCode = (int)response.StatusCode;
                    var reasonPhrase = response.ReasonPhrase;
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    // Mostrar detalles del error en la consola
                    Console.WriteLine($"Error en la API: {statusCode} {reasonPhrase}");
                    Console.WriteLine($"Detalle del error: {errorMessage}");

                    // Mostrar detalles en la vista (si es necesario)
                    TempData["Error"] = $"Error {statusCode}: {reasonPhrase}. {errorMessage}";
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                // Mostrar la excepción en la consola
                Console.WriteLine($"Excepción al conectar con la API: {ex.Message}");

                // Mostrar un mensaje genérico en la vista
                TempData["Error"] = "No se pudo conectar con el servidor. Intenta nuevamente más tarde.";
                return View(usuario);
            }
        }





        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind] Usuario usuario)
        {
            AutorizacionResponse autorizacion = null;

            if (usuario == null)
            {
                TempData["MensajeLogin"] = "Usuario o contraseña incorrectos.";
                return View(usuario);
            }

            HttpResponseMessage response = await httpClient.PostAsync($"/Usuarios/AutenticarPW?email={usuario.Email}&password={usuario.Password}", null);
            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;
                autorizacion = JsonConvert.DeserializeObject<AutorizacionResponse>(resultado);
            }

            if (autorizacion != null && autorizacion.Resultado == true)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, autorizacion.RolId.ToString()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("token", autorizacion.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["MensajeLogin"] = "Usuario o contraseña incorrectos.";
                return View(usuario);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
