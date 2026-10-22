using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Estado
    {
        // Identificador único del estado
        public int Id { get; set; }

        // Etiqueta del estado (Ej: Abierto, En proceso, Resuelto, Cerrado)
        public string Nombre { get; set; } = string.Empty;
    }
}
