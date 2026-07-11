using System.ComponentModel.DataAnnotations;

namespace DinoArgentoApi.Models.Dieta.Dto
{
    public class CreateDietaDTO
    {
        [Required(ErrorMessage = "El nombre de la dieta es requerido")]
        public string Nombre { get; set; } = null!;
    }
}
