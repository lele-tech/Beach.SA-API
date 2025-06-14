using Microsoft.AspNetCore.Mvc;
using Beach.SA.Models;
using Newtonsoft.Json;
using Beach.SA.Data;
using System.Net.Http.Headers;


namespace Beach.SA.Controllers
{
    public class AdminController : Controller
    {
        private readonly BeachApi beachAPI;
        private readonly HttpClient httpClient;

        public AdminController()
        {
            beachAPI = new BeachApi();
            httpClient = beachAPI.Inicial();
        }
        private AuthenticationHeaderValue AutorizacionToken()
        {

            var token = HttpContext.Session.GetString("token");

            AuthenticationHeaderValue authentication = null;

            if (token != null && token.Length != 0)
            {

                authentication = new AuthenticationHeaderValue("Bearer", token);
            }


            return authentication;
        }
        public async Task<IActionResult> Index()
        {
            List<Usuario> empleados = new List<Usuario>();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("/Usuarios/Listado"); 

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();

                    empleados = JsonConvert.DeserializeObject<List<Usuario>>(resultado);

                    empleados = empleados.Where(r => r.RolId == 3).ToList();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción al llamar la API: " + ex.Message);
            }

            return View(empleados);
        }

        public async Task<IActionResult> Delete(string email)
        {

            HttpResponseMessage response = await httpClient.GetAsync("/Usuarios/Listado");


            var resultado = await response.Content.ReadAsStringAsync();
            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(resultado);

            var usuario = usuarios.FirstOrDefault(r => r.Email == email);

            return View(usuario);
        }


        public async Task<IActionResult> DeleteConfirmed(string Email)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage response = await httpClient.DeleteAsync($"/Usuarios/Eliminar?email={Email}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("No se pudo eliminar la reserva.");
            }
        }



    }

}