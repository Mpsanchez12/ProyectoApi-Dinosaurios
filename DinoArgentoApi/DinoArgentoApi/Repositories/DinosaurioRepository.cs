using DinoArgentoApi.Config;
using DinoArgentoApi.Models.Dinosaurio;

namespace DinoArgentoApi.Repositories
{
    public interface IDinosaurioRepository : IRepository<Dinosaurio>
    {

    }

    public class DinosaurioRepository : Repository<Dinosaurio>, IDinosaurioRepository
    {
        private readonly AppDbContext _db;


        public DinosaurioRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
