namespace HelpDeskUnev.Models
{
    public class Cliente : Usuario
    {
        // NO vuelvas a declarar Matricula, Nombre, ContrasenaHash, etc.
        // Ya los hereda automáticamente de Usuario.
        public string? Carrera { get; set; }
    }
}