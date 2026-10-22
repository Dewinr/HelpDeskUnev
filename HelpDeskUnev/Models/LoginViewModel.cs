using System.ComponentModel.DataAnnotations;

namespace HelpDeskUnev.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "La matrícula es obligatoria.")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; } = string.Empty;
    }
}