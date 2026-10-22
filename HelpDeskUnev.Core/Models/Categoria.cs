using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Models
{
    public class Categoria
    {
        // Identificador único de la categoría
        public int Id { get; set; }

        // Nombre de la categoría (Ej: Campus Virtual, Correo Institucional, Wifi/Redes)
        public string Nombre { get; set; } = string.Empty;

        // Explicación breve sobre los servicios o problemas que abarca
        public string Descripcion { get; set; } = string.Empty;
    }
}
