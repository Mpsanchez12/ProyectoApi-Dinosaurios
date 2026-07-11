using AutoMapper;
using DinoArgentoApi.Models.Dieta;
using DinoArgentoApi.Models.Dieta.Dto;
using DinoArgentoApi.Repositories;
using DinoArgentoApi.Utils;
using System.Net;

namespace DinoArgentoApi.Services
{
    public class DietaService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Dieta> _repo;

        public DietaService(IMapper mapper, IRepository<Dieta> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<List<Dieta>> GetAll() => await _repo.GetAll();

        public async Task<Dieta> GetOneById(int id)
        {
            var d = await _repo.GetOne(x => x.Id == id);
            if (d == null) throw new ErrorResponse(HttpStatusCode.NotFound, $"Dieta {id} no encontrada");
            return d;
        }


        public async Task<List<Dieta>> GetManyByIds(List<int> ids)
        {
            var dietas = await _repo.GetAll(d => ids.Contains(d.Id));
            return dietas;
        }

        public async Task<Dieta> CreateOne(CreateDietaDTO dto)
        {
            var d = _mapper.Map<Dieta>(dto);
            return await _repo.CreateOne(d);
        }

        public async Task<Dieta> UpdateOneById(int id, UpdateDietaDTO dto)
        {
            var d = await GetOneById(id);
            var updated = _mapper.Map(dto, d);
            return await _repo.UpdateOne(updated);
        }

        public async Task DeleteOneById(int id)
        {
            var d = await GetOneById(id);
            await _repo.DeleteOne(d);
        }
    }
}
    
