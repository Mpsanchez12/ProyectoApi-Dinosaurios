using System.ComponentModel.DataAnnotations;

namespace DinoArgentoApi.Models.Dinosaurio.Dto
{
    public class CreateDinosaurioDTO
    {
        [Required(ErrorMessage = "El nombre del dinosaurio es requerido")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El periodo es requerido")]
        public int PeriodoId { get; set; }

        [Required(ErrorMessage = "Al menos una dieta es requerida")]
        public List<int> DietasIds { get; set; } = new();

        [Required(ErrorMessage = "El peso es requerido")]
        [Range(1, 100000, ErrorMessage = "El peso debe ser un valor positivo válido")]
        public double Peso { get; set; }
    }
}
