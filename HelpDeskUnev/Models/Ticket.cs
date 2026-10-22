using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskUnev.Models
{
    public class Ticket
    {
        [Key]
        public int IdTicket { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public string? Asunto { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Claves Foráneas
        public int IdEstado { get; set; }
        [ForeignKey("IdEstado")]
        public virtual Estado? Estado { get; set; }

        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoria? Categoria { get; set; }

        public int IdPrioridad { get; set; }
        [ForeignKey("IdPrioridad")]
        public virtual Prioridad? Prioridad { get; set; }

        // Relación general con la clase base Usuario (Soporta Clientes y Maestros por igual)
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Usuario? Cliente { get; set; }

        public int? IdTecnico { get; set; }
        [ForeignKey("IdTecnico")]
        public virtual Tecnico? Tecnico { get; set; }

        public virtual ICollection<ComentarioTicket> Comentarios { get; set; } = new List<ComentarioTicket>();
    }
}