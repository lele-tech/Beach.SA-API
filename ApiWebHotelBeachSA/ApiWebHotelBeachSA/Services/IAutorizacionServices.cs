using ApiWebHotelBeachSA.Models.Custom;
using ApiWebHotelBeachSA.Models;

namespace ApiWebHotelBeachSA.Services
{
    public interface IAutorizacionServices
    {
        Task<AutorizacionResponse> DevolverToken(Usuario autorizacion);
    }
}
