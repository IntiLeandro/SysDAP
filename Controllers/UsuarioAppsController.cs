using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysDAP.Data;
using SysDAP.Models;

namespace SysDAP.Controllers
{
    public class UsuarioAppsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;
        public UsuarioAppsController(ApplicationDbContext context)
        {
            _context = context;
            _passwordService = new PasswordService();
        }

        // GET: UsuarioApps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UsuariosApp.Include(u => u.Empleado);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UsuarioApps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioApp = await _context.UsuariosApp
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioApp == null)
            {
                return NotFound();
            }

            return View(usuarioApp);
        }

        // GET: UsuarioApps/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Apellido");
            return View();
        }

        // POST: UsuarioApps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioApp usuarioApp)//[Bind("Id,NombreUsuario,ContraseñaHash,Rol,EmpleadoId")] 
        {
            //usuarioApp.ContraseñaHash = _passwordService.HashPassword(initialPassword);

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error en {modelStateKey}: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                usuarioApp.ContraseñaHash = _passwordService.HashPassword(usuarioApp.ContraseñaHash);
                _context.Add(usuarioApp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Apellido", usuarioApp.EmpleadoId);
            return View(usuarioApp);
        }

        // GET: UsuarioApps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioApp = await _context.UsuariosApp.FindAsync(id);
            if (usuarioApp == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Apellido", usuarioApp.EmpleadoId);
            return View(usuarioApp);
        }

        // POST: UsuarioApps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,ContraseñaHash,Rol,EmpleadoId")] UsuarioApp usuarioApp)
        {
            if (id != usuarioApp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioApp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioAppExists(usuarioApp.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "EmpleadoId", "Apellido", usuarioApp.EmpleadoId);
            return View(usuarioApp);
        }

        // GET: UsuarioApps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioApp = await _context.UsuariosApp
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioApp == null)
            {
                return NotFound();
            }

            return View(usuarioApp);
        }

        // POST: UsuarioApps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioApp = await _context.UsuariosApp.FindAsync(id);
            if (usuarioApp != null)
            {
                _context.UsuariosApp.Remove(usuarioApp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioAppExists(int id)
        {
            return _context.UsuariosApp.Any(e => e.Id == id);
        }
    }
}
