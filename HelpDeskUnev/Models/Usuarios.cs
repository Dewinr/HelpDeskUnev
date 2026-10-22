using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskUnev.Models
{
    public abstract class Usuario : IAutenticable
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Matricula { get; set; } = string.Empty;

        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string ContrasenaHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        // RENOMBRADO A EsActivo PARA EVITAR CONFLICTO CON LA ENTIDAD 'Estado'
        public bool EsActivo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Métodos requeridos por la interfaz IAutenticable
        public virtual bool IniciarSesion(string matricula, string contrasena)
        {
            return this.Matricula == matricula && this.ContrasenaHash == contrasena;
        }

        public virtual void CerrarSesion()
        {
            // Limpieza de datos de sesión
        }

        public virtual bool ValidarSesionActiva()
        {
            return this.EsActivo;
        }
    }
}