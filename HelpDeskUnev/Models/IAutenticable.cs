namespace HelpDeskUnev.Models
{
    public interface IAutenticable
    {
        bool IniciarSesion(string matricula, string contrasena);
        void CerrarSesion();
        bool ValidarSesionActiva();
    }
}