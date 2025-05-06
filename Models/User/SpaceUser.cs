using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SpaceUserAPI.Models.User
{
    public class SpaceUser : IdentityUser
    {
        [Required(ErrorMessage = "El Campo {0} es Obligatorio"), StringLength(24, MinimumLength = 3), Display(Name = "Nombre: ")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio"), StringLength(24, MinimumLength = 3), Display(Name = "Apellido 1: ")]
        public string? Surname1 { get; set; }
        [StringLength(24, MinimumLength = 3), Display(Name = "Apellido 2: ")]
        public string? Surname2 { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Obligatorio"), Display(Name = "Fecha de Nacimiento: ")]
        public DateOnly Bday { get; set; }
        [Display(Name = "Foto de Perfil: ")]
        public string? ProfileImage { get; set; }
    }
}