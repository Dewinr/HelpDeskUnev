using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Tecnico
    {
        // Identificador único del personal técnico
        public int Id { get; set; }

        // Nombre completo del técnico de soporte
        public string Nombre { get; set; } = string.Empty;

        // Correo electrónico de contacto laboral
        public string Email { get; set; } = string.Empty;

        // Área técnica de dominio (Ej: Infraestructura de Redes, Bases de Datos)
        public string Especialidad { get; set; } = string.Empty;

        // Indica si el técnico puede asumir la atención de nuevos tickets
        public bool Disponible { get; set; } = true;
    }
}
