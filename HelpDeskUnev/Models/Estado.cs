using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskUnev.Models
{
    public class Estado
    {
        [Key]
        public int IdEstado { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Propiedades necesarias para tu DbContext y vistas
        public string? NombreEstado { get; set; }
        public string? Descripcion { get; set; }
    }
}