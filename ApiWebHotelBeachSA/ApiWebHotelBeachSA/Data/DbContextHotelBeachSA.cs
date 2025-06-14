using Microsoft.EntityFrameworkCore;

using ApiWebHotelBeachSA.Models;

namespace ApiWebHotelBeachSA.Data
{
    public class DbContextHotelBeachSA : DbContext
    {
        public DbContextHotelBeachSA(DbContextOptions<DbContextHotelBeachSA> options) : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Paquete> Paquetes { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
