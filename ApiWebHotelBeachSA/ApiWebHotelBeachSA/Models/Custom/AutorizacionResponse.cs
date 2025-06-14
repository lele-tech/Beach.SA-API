using ApiWebHotelBeachSA.Models.Custom;

namespace ApiWebHotelBeachSA.Models.Custom
{
    public class AutorizacionResponse
    {
        public string Token { get; set; }

        public bool Resultado { get; set; }

        public string Msj { get; set; }

        public int RolId { get; set; }
    }
}
