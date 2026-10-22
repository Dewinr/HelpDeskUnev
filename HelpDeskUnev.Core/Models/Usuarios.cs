using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Usuario
    {
        // Identificador único del usuario
        public int Id { get; set; }

        // Nombre completo del usuario
        public string Nombre { get; set; } = string.Empty;

        // Correo institucional (@unev.edu.do)
        public string Email { get; set; } = string.Empty;

        // Contraseña de acceso al sistema (almacenada mediante hash)
        public string Password { get; set; } = string.Empty;

        // Rol del usuario dentro del sistema: Estudiante, Profesor, Administrativo
        public string TipoUsuario { get; set; } = string.Empty;

        // Facultad o departamento académico al que pertenece
        public string Facultad { get; set; } = string.Empty;

        // Indica si la cuenta de usuario está habilitada o suspendida
        public bool Activo { get; set; } = true;
    }
}
