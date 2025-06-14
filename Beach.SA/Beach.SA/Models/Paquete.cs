using System.ComponentModel.DataAnnotations;

namespace Beach.SA.Models
{
    public class Paquete
    {
        [Key]
        public int PaqueteId { get; set; }

        [Required(ErrorMessage = "El nombre del paquete es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre del paquete no puede exceder los 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El costo por persona por noche es requerido.")]
        [Display(Name = "Costo por Persona por Noche")]
        public decimal CostoPersona { get; set; }

        [Required(ErrorMessage = "El porcentaje de prima es requerido.")]
        [Display(Name = "Prima (%)")]
        public decimal PrimaPorcentaje { get; set; }

        [Required(ErrorMessage = "El plazo en meses es requerido.")]
        [Display(Name = "Plazo en Meses")]
        public int PlazoMeses { get; set; }
    }
}
