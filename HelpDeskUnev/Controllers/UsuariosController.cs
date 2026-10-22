using HelpDeskUnev.Data;
using HelpDeskUnev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskUnev.Controllers
{
    [Authorize(Roles = "Administrador")] // 🔒 Restringido estrictamente a Administradores
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios/Index (Listado general de usuarios del sistema)
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // GET: Usuarios/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Usuarios/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(string nombre, string apellido, string correo, string matricula, string password, string tipoRol)
        {
            if (string.IsNullOrEmpty(matricula) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "La matrícula y la contraseña son obligatorias.";
                return View();
            }

            // Validar si ya existe la matrícula
            if (await _context.Usuarios.AnyAsync(u => u.Matricula == matricula))
            {
                ViewBag.Error = "Ya existe un usuario registrado con esa matrícula.";
                return View();
            }

            string hash = HelpDeskUnev.Helpers.PasswordHelper.HashPassword(password);
            Usuario nuevoUsuario;

            // Polimorfismo y ruteo según los 4 roles seleccionados en el formulario
            switch (tipoRol)
            {
                case "Maestro":
                    nuevoUsuario = new Maestro { Nombre = nombre, Apellido = apellido, Correo = correo, Matricula = matricula, ContrasenaHash = hash, EsActivo = true };
                    break;
                case "Tecnico":
                    nuevoUsuario = new Tecnico { Nombre = nombre, Apellido = apellido, Correo = correo, Matricula = matricula, ContrasenaHash = hash, EsActivo = true };
                    break;
                case "Administrador":
                    // Si el Administrador hereda de Tecnico o de Usuario según tu estructura
                    nuevoUsuario = new Tecnico { Nombre = nombre, Apellido = apellido, Correo = correo, Matricula = matricula, ContrasenaHash = hash, EsActivo = true };
                    break;
                default:
                    nuevoUsuario = new Cliente { Nombre = nombre, Apellido = apellido, Correo = correo, Matricula = matricula, ContrasenaHash = hash, EsActivo = true };
                    break;
            }

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"¡Usuario ({tipoRol}) creado exitosamente!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Usuarios/CambiarEstado (Activar / Inactivar)
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.EsActivo = !usuario.EsActivo;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Se actualizó el estado del usuario {usuario.Matricula}.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}