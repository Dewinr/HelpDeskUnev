using HelpDeskUnev.Data;
using HelpDeskUnev.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDeskUnev.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Administrador") || User.IsInRole("Tecnico"))
                {
                    return RedirectToAction("Gestionar", "Ticket");
                }

                return RedirectToAction("MisTickets", "Ticket");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string matricula, string contrasena)
        {
            string matriculaLimpia = matricula?.Trim() ?? "";
            string claveLimpia = contrasena?.Trim() ?? "";

            if (string.IsNullOrEmpty(matriculaLimpia) || string.IsNullOrEmpty(claveLimpia))
            {
                ViewBag.Error = "Debe ingresar la matrícula y la contraseña.";
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Matricula == matriculaLimpia && u.EsActivo);

            if (usuario == null)
            {
                ViewBag.Error = "Matrícula o contraseña incorrectas.";
                return View();
            }

            bool claveValida = (usuario.ContrasenaHash == claveLimpia);
            if (!claveValida)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(claveLimpia));
                    var computedHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    if (usuario.ContrasenaHash?.ToLower() == computedHash)
                    {
                        claveValida = true;
                    }
                }
            }

            if (!claveValida)
            {
                ViewBag.Error = "Matrícula o contraseña incorrectas.";
                return View();
            }

            // --- ASIGNACIÓN DE ROLES ROBUSTA Y SEGURA ---
            string rolUsuario = DeterminarRolUsuario(usuario);

            string nombreCompleto = $"{usuario.Nombre ?? ""} {usuario.Apellido ?? ""}".Trim();
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                nombreCompleto = "Usuario Institucional";
            }

            string sessionSecurityToken = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, nombreCompleto),
                new Claim(ClaimTypes.Role, rolUsuario),
                new Claim("SecurityToken", sessionSecurityToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            await HttpContext.SignInAsync(
                "CookieAuth",
                new ClaimsPrincipal(claimsIdentity));

            TempData["BienvenidaMensaje"] = $"¡Autenticación exitosa! Bienvenido/a {nombreCompleto}. Has ingresado con el rol de {rolUsuario}.";
            TempData["SecurityToken"] = sessionSecurityToken;

            if (rolUsuario == "Administrador" || rolUsuario == "Tecnico")
            {
                return RedirectToAction("Gestionar", "Ticket");
            }

            return RedirectToAction("MisTickets", "Ticket");
        }

        // --- MÉTODOS PARA INICIO DE SESIÓN CON MICROSOFT ---

        [HttpGet]
        public IActionResult ExternalLogin(string provider = "Microsoft")
        {
            // Construye la URL de retorno apuntando al Callback
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Login") ?? "/Login/ExternalLoginCallback";

            // Crea las propiedades de autenticación externa
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            // Lanza el desafío de autenticación hacia Microsoft
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.MicrosoftAccount.MicrosoftAccountDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                TempData["Error"] = "No se pudo autenticar con Microsoft.";
                return RedirectToAction(nameof(Index));
            }

            // Extraer el correo de los claims devueltos por Microsoft
            string correoMicrosoft = result.Principal.FindFirstValue(ClaimTypes.Email)
                                     ?? result.Principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            if (string.IsNullOrEmpty(correoMicrosoft))
            {
                TempData["Error"] = "La cuenta de Microsoft no proporcionó un correo válido.";
                return RedirectToAction(nameof(Index));
            }

            // Buscar el usuario en la base de datos por su correo institucional
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo != null && u.Correo.ToLower() == correoMicrosoft.ToLower() && u.EsActivo);

            if (usuario == null)
            {
                TempData["Error"] = $"El correo institucional ({correoMicrosoft}) no está registrado en el sistema HelpDesk UNEV.";
                return RedirectToAction(nameof(Index));
            }

            string rolUsuario = DeterminarRolUsuario(usuario);

            string nombreCompleto = $"{usuario.Nombre ?? ""} {usuario.Apellido ?? ""}".Trim();
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                nombreCompleto = "Usuario Institucional";
            }

            string sessionSecurityToken = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, nombreCompleto),
                new Claim(ClaimTypes.Role, rolUsuario),
                new Claim("SecurityToken", sessionSecurityToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

            TempData["BienvenidaMensaje"] = $"¡Bienvenido/a {nombreCompleto}! Acceso institucional vía Microsoft exitoso.";
            TempData["SecurityToken"] = sessionSecurityToken;

            if (rolUsuario == "Administrador" || rolUsuario == "Tecnico")
            {
                return RedirectToAction("Gestionar", "Ticket");
            }

            return RedirectToAction("MisTickets", "Ticket");
        }

        // --- MÉTODO AUXILIAR PARA EVITAR DUPLICAR LÓGICA DE ROLES ---
        private string DeterminarRolUsuario(Usuario usuario)
        {
            if (usuario.Matricula?.Trim().ToLower() == "admin" || usuario.Correo?.Trim().ToLower() == "admin@unev.edu.do")
            {
                return "Administrador";
            }

            if (usuario is Tecnico)
            {
                return "Tecnico";
            }

            if (usuario is Maestro)
            {
                return "Maestro";
            }

            string tipoClase = usuario.GetType().Name;
            if (tipoClase.Equals("Tecnico", StringComparison.OrdinalIgnoreCase))
            {
                return "Tecnico";
            }

            if (tipoClase.Equals("Maestro", StringComparison.OrdinalIgnoreCase))
            {
                return "Maestro";
            }

            return "Cliente";
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            ViewBag.Error = "No tienes los permisos o el rol necesario para acceder a esta sección del sistema.";
            return View();
        }
    }
}