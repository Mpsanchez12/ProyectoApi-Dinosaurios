using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinoArgentoApi.Models.Dieta
{
    public class Dieta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public List<Dinosaurio.Dinosaurio> Dinosaurios { get; set; } = new();
    }
}

