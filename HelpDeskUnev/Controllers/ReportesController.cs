using HelpDeskUnev.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskUnev.Controllers
{
    [Authorize(Roles = "Administrador,Tecnico")]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reportes/Index
        public async Task<IActionResult> Index()
        {
            // Incluimos la propiedad de navegación 'Estado' para evitar NullReferenceException
            var tickets = await _context.Tickets
                .Include(t => t.Estado)
                .ToListAsync();

            ViewBag.TotalTickets = tickets.Count;

            // Conteo seguro validando que t.Estado no sea null y comparando por NombreEstado o ID
            ViewBag.TicketsAbiertos = tickets.Count(t =>
                t.Estado != null &&
                (t.Estado.NombreEstado.Contains("Abierto", StringComparison.OrdinalIgnoreCase) ||
                 t.Estado.NombreEstado.Contains("Pendiente", StringComparison.OrdinalIgnoreCase) ||
                 t.Estado.NombreEstado.Contains("Proceso", StringComparison.OrdinalIgnoreCase)));

            ViewBag.TicketsResueltos = tickets.Count(t =>
                t.Estado != null &&
                t.Estado.NombreEstado.Contains("Resuelto", StringComparison.OrdinalIgnoreCase));

            ViewBag.TotalUsuarios = await _context.Usuarios.CountAsync();

            return View();
        }
    }
}