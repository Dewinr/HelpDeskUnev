#if ANDROID || IOS || MACCATALYST || WINDOWS
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models.Entities
{
    public class Ticket
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Prioridad { get; set; } = "Media"; // Alta, Media, Baja
        public string Estado { get; set; } = "Abierto";  // Abierto, En Proceso, Cerrado
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
#endif
