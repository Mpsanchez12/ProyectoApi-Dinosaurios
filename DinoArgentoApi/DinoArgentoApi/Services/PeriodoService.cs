using AutoMapper;
using DinoArgentoApi.Models.Periodo;
using DinoArgentoApi.Models.Periodo.Dto;
using DinoArgentoApi.Repositories;
using DinoArgentoApi.Utils;
using System.Net;

namespace DinoArgentoApi.Services
{
    public class PeriodoService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Periodo> _repo;

        public PeriodoService(IMapper mapper, IRepository<Periodo> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<List<Periodo>> GetAll() => await _repo.GetAll();

        public async Task<Periodo> GetOneById(int id)
        {
            var p = await _repo.GetOne(x => x.Id == id);
            if (p == null) throw new ErrorResponse(HttpStatusCode.NotFound, $"Periodo {id} no encontrado");
            return p;
        }

        public async Task<Periodo> CreateOne(PeriodoDTO dto)
        {
            var p = _mapper.Map<Periodo>(dto);
            return await _repo.CreateOne(p);
        }

        public async Task<Periodo> UpdateOneById(int id, PeriodoDTO dto)
        {
            var p = await GetOneById(id);
            var updated = _mapper.Map(dto, p);
            return await _repo.UpdateOne(updated);
        }

        public async Task DeleteOneById(int id)
        {
            var p = await GetOneById(id);
            await _repo.DeleteOne(p);
        }
    }
}
    