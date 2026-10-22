using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskUnev.Models
{
    public class HistorialEstados
    {
        [Key]
        public int IdHistorial { get; set; }

        public int IdTicket { get; set; }
        [ForeignKey("IdTicket")]
        public virtual Ticket? Ticket { get; set; }

        // Propiedades que faltaban:
        public int? IdEstadoAnterior { get; set; }

        public int IdEstadoNuevo { get; set; }

        public int IdModificadoPor { get; set; }

        public DateTime FechaCambio { get; set; } = DateTime.Now;
    }
}