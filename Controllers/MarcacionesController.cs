using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysDAP.Models;
using SysDAP.Data.Migrations;
using SysDAP.Data;
using Microsoft.Data.SqlClient; // Reemplaza con el namespace de tus modelos

namespace SysDAP.Controllers
{
    public class MarcacionesController : Controller
    {
        private readonly MarcacionesService _marcacionesService;

        public MarcacionesController(MarcacionesService marcacionesService)
        {
            _marcacionesService = marcacionesService;
        }

        // Acción para ver todas las marcaciones
        //public async Task<IActionResult> Index()
        //{
        //    var marcaciones = await _marcacionesService.GetAllMarcacionesAsync();
        //    return View(marcaciones);  // Se pasa la lista de marcaciones a la vista
        //}

        public async Task<ActionResult> Index(MarcacionFiltro filtro)
        {
            // Crear lista para almacenar los resultados
            List<Marcaciones> marcaciones = new List<Marcaciones>();

            // Crear la consulta SQL con parámetros dinámicos
            string query = "SELECT m.EmpleadoId, m.Fecha, m.HoraEntrada, m.HoraSalida " +
                           "FROM Marcaciones m " +
                           "INNER JOIN Empleados e ON m.EmpleadoId = e.EmpleadoId " +
                           "WHERE 1=1"; // Condición base

            var parameters = new List<SqlParameter>();

            if (filtro.EmpleadoID.HasValue)
            {
                query += " AND m.EmpleadoId = @EmpleadoID";
                parameters.Add(new SqlParameter("@EmpleadoID", filtro.EmpleadoID.Value));
            }

            if (filtro.FechaDesde.HasValue)
            {
                query += " AND m.Fecha >= @FechaDesde";
                parameters.Add(new SqlParameter("@FechaDesde", filtro.FechaDesde.Value));
            }

            if (filtro.FechaHasta.HasValue)
            {
                query += " AND m.Fecha <= @FechaHasta";
                parameters.Add(new SqlParameter("@FechaHasta", filtro.FechaHasta.Value));
            }
            //private const string ConnectionString = "Server=192.168.100.9,1433;Database=prueba3;User Id=inti;Password=1234;Encrypt=False;";
            //<add name="DefaultConnection" connectionString="Data Source=LAPTOP-4N9B4VNR;Initial Catalog=DBDAP;Integrated Security=True" providerName="System.Data.SqlClient" />
            using (SqlConnection connection = new SqlConnection("Server=LAPTOP-4N9B4VNR;Database=prueba3;User Id=inti;Password=1234;Encrypt=False;"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        marcaciones.Add(new Marcaciones
                        {
                            //MarcacionID = Convert.ToInt32(reader["MarcacionID"]),
                            EmpleadoId = Convert.ToInt32(reader["EmpleadoId"]),
                            //EmpleadoNombre = reader["EmpleadoNombre"].ToString(),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            HoraEntrada = TimeSpan.Parse(reader["HoraEntrada"].ToString()),
                            HoraSalida = TimeSpan.Parse(reader["HoraSalida"].ToString())
                        });
                    }
                }
            }

            // Pasar los resultados y los filtros a la vista
            ViewBag.Filtros = filtro;
            return View(marcaciones);
        }

        // Acción para ver las marcaciones de un empleado específico
        public async Task<IActionResult> EmployeeMarcaciones(int id)
        {
            var marcaciones = await _marcacionesService.GetMarcacionesByEmployeeIdAsync(id);
            return View(marcaciones);  // Se pasa la lista de marcaciones a la vista
        }
    }
}
