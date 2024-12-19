using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SysDAP.Models;

namespace SysDAP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<UsuarioApp> UsuariosApp { get; set; }
        public DbSet<Marcaciones> Marcaciones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura la clave primaria para Marcaciones
            modelBuilder.Entity<Marcaciones>()
                .HasKey(m => new { m.EmpleadoId, m.Fecha });
        }
    }
    }
