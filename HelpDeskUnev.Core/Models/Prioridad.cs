using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Prioridad
    {
        // Identificador único del nivel de prioridad
        public int Id { get; set; }

        // Denominación del impacto (Ej: Baja, Media, Alta, Urgente)
        public string Nombre { get; set; } = string.Empty;
    }
}
