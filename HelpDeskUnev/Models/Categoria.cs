using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskUnev.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Propiedades necesarias para tu DbContext y vistas
        public string? NombreCategoria { get; set; }
        public string? Descripcion { get; set; }
    }
}