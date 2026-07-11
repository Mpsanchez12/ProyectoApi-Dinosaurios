using System.ComponentModel.DataAnnotations;

namespace DinoArgentoApi.Models.User.Dto
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El usuario o email es requerido")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = null!;
    }
}
