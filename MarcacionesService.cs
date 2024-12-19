using Microsoft.EntityFrameworkCore;
using SysDAP.Data;
using SysDAP.Models;

namespace SysDAP
{
    public class MarcacionesService
    {
        private readonly ApplicationDbContext _context;

        public MarcacionesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener todas las marcaciones
        public async Task<List<Marcaciones>> GetAllMarcacionesAsync()
        {
            return await _context.Marcaciones.ToListAsync();
        }

        // Método para obtener las marcaciones de un empleado específico
        public async Task<List<Marcaciones>> GetMarcacionesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Marcaciones
                                 .Where(m => m.EmpleadoId == employeeId)
                                 .ToListAsync();
        }
    }
}
