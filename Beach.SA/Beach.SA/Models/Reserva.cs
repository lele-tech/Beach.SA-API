using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beach.SA.Models
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }


        [Required(ErrorMessage = "La cédula del cliente es requerida.")]
        [Display(Name = "Cédula del Cliente")]
        public int CedCliente { get; set; }

        [NotMapped]
        public List<Paquete>? PaquetesDisponibles { get; set; }

        [Required(ErrorMessage = "El paquete es requerido.")]
        [Display(Name = "Paquete")]
        public int PaqueteId { get; set; }


        [Required(ErrorMessage = "La cantidad de noches es requerida.")]
        [Range(1, 365, ErrorMessage = "La cantidad de noches debe estar entre 1 y 365.")]
        [Display(Name = "Cantidad de Noches")]
        public int CantidadNoches { get; set; }


        [Required]
        [Display(Name = "Monto Total (Colones)")]
        public decimal MontoTotalColones { get; set; }


        [Required]
        [Display(Name = "Monto Total (Dólares)")]
        public decimal MontoTotalDolares { get; set; }


        [Required]
        [Display(Name = "Monto Prima")]
        public decimal MontoPrima { get; set; }


        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }


        [Required]
        [Display(Name = "Fecha de Reserva")]
        public DateTime FechaReserva { get; set; } = DateTime.UtcNow;


        [MaxLength(50, ErrorMessage = "El método de pago no puede exceder los 50 caracteres.")]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; }


        [MaxLength(20, ErrorMessage = "El número de cheque no puede exceder los 20 caracteres.")]
        [Display(Name = "Número de Cheque")]
        public string NumeroCheque { get; set; }


        [MaxLength(50, ErrorMessage = "El nombre del banco no puede exceder los 50 caracteres.")]
        [Display(Name = "Banco")]
        public string Banco { get; set; }


        [Display(Name = "Reserva Activa")]
        public char Activa { get; set; }
    }
}
