using System.ComponentModel.DataAnnotations;

namespace HelpDeskUnev.Models
{
    public class Prioridad
    {
        [Key]
        public int IdPrioridad { get; set; }

        [Required]
        [StringLength(50)]
        public string Nivel { get; set; } = string.Empty;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}