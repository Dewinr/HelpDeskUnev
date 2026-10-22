namespace HelpDeskUnev.Models
{
    public class DashboardViewModel
    {
        // 1. Tarjetas de Indicadores (KPIs)
        public int TotalTickets { get; set; }
        public int TicketsPendientes { get; set; }
        public int TicketsEnProceso { get; set; }
        public int TicketsResueltos { get; set; }

        // 2. Datos para Gráficos (ej. Distribución por Categoría o Estado)
        public List<string> CategoriasLabels { get; set; } = new List<string>();
        public List<int> CategoriasConteo { get; set; } = new List<int>();

        public List<string> EstadosLabels { get; set; } = new List<string>();
        public List<int> EstadosConteo { get; set; } = new List<int>();

        // 3. Tabla de Actividad Reciente / Últimos Movimientos
        public List<TicketDto> UltimosTickets { get; set; } = new List<TicketDto>();
    }

    public class TicketDto
    {
        public int IdTicket { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public string Creador { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}