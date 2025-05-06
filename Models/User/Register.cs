using System.ComponentModel.DataAnnotations;

namespace SpaceUserAPI.Models.User
{
    public class Register
    {
        public String? Id { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio."), Display(Name = "Nombre: ")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio."), Display(Name = "Apellido 1: ")]
        public string? Surname1 { get; set; }
        [Display(Name = "Apellido 2: ")]
        public string? Surname2 { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio."), DataType(DataType.EmailAddress), Display(Name = "E-mail: ")]
        public string? Email { get; set; }
        [DataType(DataType.Password), Display(Name = "Contraseña: ")]
        public string? Password { get; set; }
        [DataType(DataType.Password), Compare("Password", ErrorMessage = "Las Contraseñas no Coinciden, Por Favor Escribelas de Nuevo."), Display(Name = "Repite Contraseña: ")]
        public string? Password2 { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio."), Display(Name = "Teléfono: ")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio."), DataType(DataType.Date), Display(Name = "Fecha de Nacimiento: ")]
        public DateOnly Bday { get; set; }
        [Display(Name = "Foto de Perfil: ")]
        public IFormFile? ProfileImageFile { get; set; }
    }
}