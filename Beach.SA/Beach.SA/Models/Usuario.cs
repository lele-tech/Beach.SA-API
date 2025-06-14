using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Beach.SA.Models
{
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage = "Debe ingresar el email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "No se permite el nombre en blanco")]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "No se permite el password en blanco")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool Confirmar(string pw)
        {
            bool confirmado = false;
            if (Password != null)
            {
                if (Password.Equals(pw))
                {
                    confirmado = true;
                }
            }
            return confirmado;
        }



        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;


        public int RolId { get; set; } = 3;
    }
}
