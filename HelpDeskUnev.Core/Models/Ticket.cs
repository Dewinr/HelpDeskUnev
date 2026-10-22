using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Ticket
    {
        // Identificador único del ticket de soporte
        public int Id { get; set; }

        // Asunto o título breve de la incidencia
        public string Titulo { get; set; } = string.Empty;

        // Explicación detallada del problema o requerimiento
        public string Descripcion { get; set; } = string.Empty;

        // Fecha y hora en que se registró la solicitud
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Clave foránea que vincula al Usuario que reportó la incidencia
        public int IdUsuario { get; set; }

        // Clave foránea opcional del Técnico asignado para resolver el problema
        public int? IdTecnico { get; set; }

        // Clave foránea del área/servicio afectado (Ej: Campus Virtual, Wifi)
        public int IdCategoria { get; set; }

        // Clave foránea del estado actual (Ej: Abierto, En proceso, Resuelto)
        public int IdEstado { get; set; }

        // Clave foránea del nivel de urgencia del ticket (Ej: Baja, Alta)
        public int IdPrioridad { get; set; }
    }
}
