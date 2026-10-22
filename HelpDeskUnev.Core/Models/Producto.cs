#if ANDROID || IOS || MACCATALYST || WINDOWS
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
#endif
