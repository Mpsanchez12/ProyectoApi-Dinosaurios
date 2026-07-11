namespace DinoArgentoApi.Models.Dinosaurio.Dto
{
    public class DinosaurioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool IsActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<string> Dietas { get; set; } = new();
        public string Periodo { get; set; } = null!;
        public int PeriodoId { get; set; }

        public double Peso { get; set; }
    }
}
