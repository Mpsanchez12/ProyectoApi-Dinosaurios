using AutoMapper;
using DinoArgentoApi.Models.Dinosaurio;
using DinoArgentoApi.Models.Dinosaurio.Dto;
using DinoArgentoApi.Repositories;
using DinoArgentoApi.Utils;
using System.Net;

namespace DinoArgentoApi.Services
{
    public class DinosaurioService
    {
        private readonly IMapper _mapper;
        private readonly IDinosaurioRepository _repo;
        private readonly PeriodoService _periodoService;
        private readonly DietaService _dietaService;


        public DinosaurioService(IMapper mapper, PeriodoService periodoService, DietaService dietaService, IDinosaurioRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
            _periodoService = periodoService;
            _dietaService = dietaService;
        }

        public async Task<List<DinosaurioDTO>> GetAll()
        {
            var lista = await _repo.GetAll();
            var dinos = _mapper.Map<List<DinosaurioDTO>>(lista);

            return dinos;
        }


        private async Task<Dinosaurio> _GetOneById(int id)
        {
            var dinosaurio = await _repo.GetOne(e => e.Id == id);

            if (dinosaurio == null)
            {
                throw new ErrorResponse(
                    HttpStatusCode.NotFound,
                    $"Espécimen con ID = {id} no encontrado en los registros del parque"
                );
            }
            return dinosaurio;
        }

        public async Task<DinosaurioDTO> GetOneById(int id)
        {
            var dino = await _GetOneById(id);
            var dto = _mapper.Map<DinosaurioDTO>(dino);
            return dto;
        }

        public async Task<Dinosaurio> CreateOne(CreateDinosaurioDTO dino)
        {

            var d = _mapper.Map<Dinosaurio>(dino);


            int periodoId = dino.PeriodoId;
            var periodo = await _periodoService.GetOneById(periodoId);
            d.Periodo = periodo;


            var dietas = await _dietaService.GetManyByIds(dino.DietasIds);
            d.Dietas = dietas;

            return await _repo.CreateOne(d);
        }

        public async Task<Dinosaurio> UpdateOneById(int id, UpdateDinosaurioDTO updateDto)
        {
            var dino = await _GetOneById(id);


            if (updateDto.PeriodoId != null)
            {
                int periodoId = (int)updateDto.PeriodoId;
                var periodo = await _periodoService.GetOneById(periodoId);
                dino.Periodo = periodo;
            }


            if (updateDto.DietasIds != null)
            {
                var dietas = await _dietaService.GetManyByIds(updateDto.DietasIds);
                dino.Dietas = dietas;
            }


            var updated = _mapper.Map(updateDto, dino);

            return await _repo.UpdateOne(updated);
        }

        public async Task DeleteOneById(int id)
        {
            var dino = await _GetOneById(id);
            await _repo.DeleteOne(dino);
        }
    }
}
    