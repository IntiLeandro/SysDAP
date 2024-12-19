using System.ComponentModel.DataAnnotations;
namespace SysDAP.Models
{
    public class Marcaciones
    {
        public int EmpleadoId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
    }
}
