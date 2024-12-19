using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SysDAP.Models
{
    public class UsuarioApp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }

        
        [StringLength(255)]
        public string ContraseñaHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Rol { get; set; }

        public int EmpleadoId { get; set; }
        public virtual Empleado? Empleado { get; set; }

    }
}
