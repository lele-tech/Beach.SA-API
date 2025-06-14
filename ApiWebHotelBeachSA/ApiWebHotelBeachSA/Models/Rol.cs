using System.ComponentModel.DataAnnotations;

namespace ApiWebHotelBeachSA.Models
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Descripcion { get; set; }
    }
}
