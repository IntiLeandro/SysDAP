using Microsoft.AspNetCore.Mvc;
using SysDAP.Data;
using SysDAP.Models;
using System.Linq;

namespace SysDAP.Controllers
{
    public class HorasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HorasController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            // Envía un modelo vacío para inicializar la vista
            return View(new List<HorasTrabajadasViewModel>());
        }

        [HttpPost]
        public IActionResult Index(int mes, int anio)
        {
            // Obtener todas las marcaciones filtradas por mes y año
            var marcaciones = _context.Marcaciones
                .Where(m => m.Fecha.Month == mes && m.Fecha.Year == anio)
                .ToList(); // Trae los datos a memoria

            // Agrupar en memoria
            var resultado = marcaciones
                .GroupBy(m => new { m.EmpleadoId })
                .Select(g => new HorasTrabajadasViewModel
                {
                    EmpleadoId = g.Key.EmpleadoId,
                    //NombreEmpleado = g.Key.Nombre,
                    DiasTrabajados = g.Select(m => m.Fecha.Date).Distinct().Count(),
                    HorasTrabajadas = g.Sum(m => (m.HoraSalida - m.HoraEntrada).TotalHours)
                })
                .ToList();

            return View(resultado);
        }
    }
}
