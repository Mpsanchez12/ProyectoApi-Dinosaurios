using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinoArgentoApi.Models.Dinosaurio
{
    public class Dinosaurio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool IsActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int PeriodoId { get; set; }

        [ForeignKey(nameof(PeriodoId))]
        public Periodo.Periodo Periodo { get; set; } = null!;

        public List<Dieta.Dieta> Dietas { get; set; } = new();
        public double Peso { get; set; }


        
    }
}
    
