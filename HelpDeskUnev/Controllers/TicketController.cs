using HelpDeskUnev.Data;
using HelpDeskUnev.Models;
using HelpDeskUnev.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDeskUnev.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public TicketController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirigirSegunRol();
        }

        // --- ACCIÓN DEL DASHBOARD (EXCLUSIVO PARA ADMINISTRADOR) ---
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Dashboard()
        {
            var queryTickets = _context.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .Include(t => t.Categoria)
                .Include(t => t.Cliente)
                .AsQueryable();

            var ticketsList = await queryTickets.ToListAsync();

            var model = new DashboardViewModel
            {
                TotalTickets = ticketsList.Count,
                TicketsPendientes = ticketsList.Count(t => t.Estado?.NombreEstado?.ToLower() == "pendiente" || t.IdEstado == 1),
                TicketsEnProceso = ticketsList.Count(t => t.Estado?.NombreEstado?.ToLower() == "en proceso" || t.IdEstado == 2),
                TicketsResueltos = ticketsList.Count(t => t.Estado?.NombreEstado?.ToLower() == "resuelto" || t.Estado?.NombreEstado?.ToLower() == "cerrado" || t.IdEstado >= 3),

                CategoriasLabels = ticketsList.GroupBy(t => t.Categoria?.NombreCategoria ?? "General")
                                             .Select(g => g.Key).ToList(),
                CategoriasConteo = ticketsList.GroupBy(t => t.Categoria?.NombreCategoria ?? "General")
                                             .Select(g => g.Count()).ToList(),

                EstadosLabels = ticketsList.GroupBy(t => t.Estado?.NombreEstado ?? "Desconocido")
                                           .Select(g => g.Key).ToList(),
                EstadosConteo = ticketsList.GroupBy(t => t.Estado?.NombreEstado ?? "Desconocido")
                                         .Select(g => g.Count()).ToList(),

                UltimosTickets = ticketsList.OrderByDescending(t => t.FechaCreacion)
                    .Take(5)
                    .Select(t => new TicketDto
                    {
                        IdTicket = t.IdTicket,
                        Titulo = t.Titulo,
                        Estado = t.Estado?.NombreEstado ?? "N/D",
                        Prioridad = t.Prioridad?.Nivel ?? "N/D",
                        Creador = t.Cliente != null ? $"{t.Cliente.Nombre} {t.Cliente.Apellido}" : "Institucional",
                        FechaCreacion = t.FechaCreacion
                    }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Cliente,Usuario,Tecnico,Maestro")]
        public async Task<IActionResult> MisTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.ActionToken = GenerarTokenOperacion();

            var misTickets = await _context.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Where(t => t.IdCliente == usuarioId)
                .ToListAsync();

            return View(misTickets ?? new List<Ticket>());
        }

        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> Gestionar()
        {
            ViewBag.ActionToken = GenerarTokenOperacion();

            var todosLosTickets = await _context.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Cliente)
                .Include(t => t.Tecnico)
                .OrderByDescending(t => t.FechaCreacion)
                .ToListAsync();

            return View(todosLosTickets ?? new List<Ticket>());
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Tecnico,Cliente,Usuario,Maestro")]
        public async Task<IActionResult> Crear()
        {
            var listaCategorias = await _context.Categorias.ToListAsync();
            var listaPrioridades = await _context.Prioridades.ToListAsync();

            ViewBag.Categorias = new SelectList(listaCategorias, "IdCategoria", "NombreCategoria");
            ViewBag.Prioridades = new SelectList(listaPrioridades, "IdPrioridad", "Nivel");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico,Cliente,Usuario,Maestro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Ticket ticket)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return RedirectToAction("Index", "Login");
            }

            ticket.IdCliente = usuarioId;
            ticket.FechaCreacion = DateTime.Now;
            ticket.IdEstado = 1;

            if (ticket.IdCategoria == 0) ticket.IdCategoria = 1;
            if (ticket.IdPrioridad == 0) ticket.IdPrioridad = 1;

            if (string.IsNullOrEmpty(ticket.Asunto) && !string.IsNullOrEmpty(ticket.Titulo))
            {
                ticket.Asunto = ticket.Titulo;
            }
            else if (string.IsNullOrEmpty(ticket.Titulo) && !string.IsNullOrEmpty(ticket.Asunto))
            {
                ticket.Titulo = ticket.Asunto;
            }

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Ticket registrado exitosamente.";

            return RedirigirSegunRol();
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Tecnico,Cliente,Usuario,Maestro")]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Estado)
                .Include(t => t.Categoria)
                .Include(t => t.Prioridad)
                .Include(t => t.Cliente)
                .Include(t => t.Tecnico)
                .FirstOrDefaultAsync(m => m.IdTicket == id);

            if (ticket == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente") || User.IsInRole("Usuario") || User.IsInRole("Maestro"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int usuarioId) && ticket.IdCliente != usuarioId)
                {
                    return Forbid();
                }
            }

            var listaTecnicos = await _context.Usuarios
                .OfType<Tecnico>()
                .Select(u => new {
                    u.IdUsuario,
                    NombreCompleto = u.Nombre + (u.Apellido != null ? " " + u.Apellido : "")
                })
                .ToListAsync();

            ViewBag.Estados = new SelectList(await _context.Estados.ToListAsync(), "IdEstado", "NombreEstado", ticket.IdEstado);
            ViewBag.Tecnicos = new SelectList(listaTecnicos, "IdUsuario", "NombreCompleto", ticket.IdTecnico);

            return View(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarEstado(int idTicket, int idEstado, int? idTecnico)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Cliente)
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(t => t.IdTicket == idTicket);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.IdEstado = idEstado;
            ticket.IdTecnico = idTecnico;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            var estadoActualizado = await _context.Estados.FindAsync(idEstado);
            string nombreNuevoEstado = estadoActualizado?.NombreEstado ?? "Actualizado";

            if (ticket.Cliente != null && !string.IsNullOrEmpty(ticket.Cliente.Correo))
            {
                await _emailService.EnviarCorreoEstadoTicketAsync(
                    ticket.Cliente.Correo,
                    $"{ticket.Cliente.Nombre} {ticket.Cliente.Apellido}",
                    ticket.IdTicket,
                    nombreNuevoEstado
                );
            }

            TempData["Mensaje"] = "Estado del ticket actualizado y notificación enviada por correo.";

            return RedirectToAction(nameof(Gestionar));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico,Cliente,Usuario,Maestro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConToken(int idTicket, string tokenAutorizacion)
        {
            string? tokenEsperado = TempData["SecurityToken"] as string;

            if (string.IsNullOrEmpty(tokenAutorizacion) || tokenAutorizacion != tokenEsperado)
            {
                TempData["Error"] = "Token de seguridad inválido o expirado. Operación denegada.";
                return RedirigirSegunRol();
            }

            var ticket = await _context.Tickets.FindAsync(idTicket);
            if (ticket != null)
            {
                if (User.IsInRole("Cliente") || User.IsInRole("Usuario") || User.IsInRole("Maestro"))
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out int usuarioId) && ticket.IdCliente != usuarioId)
                    {
                        return Forbid();
                    }
                }

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Operación autorizada por Token. Registro eliminado exitosamente.";
            }

            return RedirigirSegunRol();
        }

        private string GenerarTokenOperacion()
        {
            string token = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            TempData["SecurityToken"] = token;
            return token;
        }

        private IActionResult RedirigirSegunRol()
        {
            if (User.IsInRole("Administrador") || User.IsInRole("Tecnico"))
            {
                return RedirectToAction(nameof(Gestionar));
            }

            return RedirectToAction(nameof(MisTickets));
        }
    }
}