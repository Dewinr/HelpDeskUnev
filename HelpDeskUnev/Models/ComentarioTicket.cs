using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskUnev.Models
{
    public class ComentarioTicket
    {
        [Key]
        public int IdComentario { get; set; }

        public int IdTicket { get; set; }
        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        public int IdAutor { get; set; }
        [ForeignKey("IdAutor")]
        public virtual Usuario? Autor { get; set; }

        [Required]
        public string Contenido { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}