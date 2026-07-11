namespace DinoArgentoApi.Models.Dinosaurio.Dto
{
    public class UpdateDinosaurioDTO
    {
        public string? Nombre { get; set; }
        public int? PeriodoId { get; set; }
        public List<int>? DietasIds { get; set; }
        public double? Peso { get; set; }
    }
}
