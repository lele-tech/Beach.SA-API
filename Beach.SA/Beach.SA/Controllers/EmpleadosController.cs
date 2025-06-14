using Microsoft.AspNetCore.Mvc;
using Beach.SA.Models;
using Newtonsoft.Json;
using Beach.SA.Data;
using System.Text;
using System.Net.Http.Headers;
using Beach.SA.Models.Custom;

namespace Beach.SA.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly BeachApi beachAPI;
        private readonly HttpClient httpClient;

        public EmpleadosController()
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

            List<Cliente> clientes = new List<Cliente>();
            HttpResponseMessage response = await httpClient.GetAsync("/Clientes/Listado");

            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;

                clientes = JsonConvert.DeserializeObject<List<Cliente>>(resultado);
            }

            return View(clientes);
        }


        public async Task<IActionResult> ListReservas(int Cedula)
        {
            List<Reserva> reservas = new List<Reserva>();

            HttpResponseMessage response = await httpClient.GetAsync("/Reservas/Listado");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                reservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);

                reservas = reservas.Where(r => r.CedCliente == Cedula).ToList();
            }

            ViewData["Cedula"] = Cedula;

            return View(reservas);
        }

        public async Task<IActionResult> Create(int id)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var response = await httpClient.GetAsync("/Paquetes/Listado");
            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var paquetes = JsonConvert.DeserializeObject<List<Paquete>>(resultado);

                var reserva = new Reserva
                {
                    PaquetesDisponibles = paquetes,
                    CedCliente = id
                };

                return View(reserva);
            }

            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();
            if (ModelState.IsValid)
            {
                try
                {
                    reserva.ReservaId = 0;

                    var jsonContent = JsonConvert.SerializeObject(reserva);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync("/Reservas/Agregar", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hubo un problema al comunicarse con la API: " + ex.Message);
                }
            }

            return View(reserva);
        }


        public async Task<IActionResult> Delete(int id)
        {

            HttpResponseMessage response = await httpClient.GetAsync("/Reservas/Listado");


            var resultado = await response.Content.ReadAsStringAsync();
            var reservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);

            var reserva = reservas.FirstOrDefault(r => r.ReservaId == id);

            return View(reserva);
        }

        public async Task<IActionResult> DeleteConfirmed(int ReservaId)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage response = await httpClient.DeleteAsync($"/Reservas/Eliminar?id={ReservaId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("No se pudo eliminar la reserva.");
            }
        }

        public async Task<IActionResult> Details(int id)
        {

            HttpResponseMessage response = await httpClient.GetAsync("/Reservas/Listado");


            var resultado = await response.Content.ReadAsStringAsync();
            var reservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);

            var reserva = reservas.FirstOrDefault(r => r.ReservaId == id);

            return View(reserva);
        }

        public async Task<IActionResult> Edit(int id, int cedula)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var respuestaReserva = await httpClient.GetAsync("/Reservas/Listado");
            var respuestaPaquete = await httpClient.GetAsync("/Paquetes/Listado");

            if (respuestaReserva.IsSuccessStatusCode && respuestaPaquete.IsSuccessStatusCode)
            {

                var resultadoReserva = await respuestaReserva.Content.ReadAsStringAsync();
                var resultadoPaquete = await respuestaPaquete.Content.ReadAsStringAsync();


                var reservas = JsonConvert.DeserializeObject<List<Reserva>>(resultadoReserva);
                var paquetes = JsonConvert.DeserializeObject<List<Paquete>>(resultadoPaquete);


                var reserva = reservas.FirstOrDefault(r => r.ReservaId == id);

                if (reserva == null)
                {
                    return NotFound();
                }

                reserva.PaquetesDisponibles = paquetes;
                reserva.CedCliente = cedula;

                return View(reserva);
            }

            return NotFound();
        }

        public async Task<IActionResult> EditConfirmed(Reserva reserva)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var jsonContent = JsonConvert.SerializeObject(reserva);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PutAsync($"/Reservas/Editar/", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("No se pudo editar la reserva.");
            }
        }



        public async Task<IActionResult> CreateClients(string Cedula)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();

            try
            {
                var response = await httpClient.GetAsync($"https://apis.gometa.org/cedulas/{Cedula}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    dynamic apiResponse = JsonConvert.DeserializeObject(jsonResponse);

                    if (apiResponse != null && apiResponse.results != null && apiResponse.results.Count > 0)
                    {
                        var apiCliente = apiResponse.results[0];

                        var cliente = new Cliente
                        {
                            Cedula = Convert.ToInt32(apiCliente.cedula),  
                            NombreCompleto = apiCliente.fullname,
                            TipoCedula = apiCliente.guess_type,  
                        };

                        return View(cliente);
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "No se encontró la información para esta cédula.";
                        return View();
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Error al consultar la cédula en la API.";
                    return View();
                }
            }
            catch (Exception ex)
            {
           
                ViewData["ErrorMessage"] = $"Ocurrió un error: {ex.Message}";
                return View();
            }
        }



        public async Task<IActionResult> ConfirmClients(Cliente cliente)
        {
            httpClient.DefaultRequestHeaders.Authorization = AutorizacionToken();
            var jsonContent = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("/Clientes/Agregar/", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("No se pudo agregar el cliente.");
            }
        }



    }
}
