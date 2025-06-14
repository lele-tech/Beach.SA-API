using System.ComponentModel.DataAnnotations;

namespace Beach.SA.Models
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "La cédula es requerida.")]
        [Display(Name = "Cédula")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El tipo de cédula es requerido.")]
        [MaxLength(20, ErrorMessage = "El tipo de cédula no puede exceder los 20 caracteres.")]
        [Display(Name = "Tipo de Cédula")]
        public string TipoCedula { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [Display(Name = "Teléfono")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es requerida.")]
        [MaxLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
}
