using System.ComponentModel.DataAnnotations;
namespace SysDAP.Models
{
    public class Empleado
    {
        [Key]
        public int EmpleadoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(10)]
        public string Cedula { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }

    }
}
