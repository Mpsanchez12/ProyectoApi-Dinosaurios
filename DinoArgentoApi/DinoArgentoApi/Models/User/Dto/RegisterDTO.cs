using System.ComponentModel.DataAnnotations;

namespace DinoArgentoApi.Models.User.Dto
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Nombre de usuario requerido")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Contraseña requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Teléfono requerido")]
        public string PhoneNumber { get; set; } = null!;
    }
}
