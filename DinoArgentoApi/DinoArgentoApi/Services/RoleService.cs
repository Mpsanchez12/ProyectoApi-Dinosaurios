using AutoMapper;
using DinoArgentoApi.Models.Role;
using DinoArgentoApi.Models.Role.Dto;
using DinoArgentoApi.Repositories;
using DinoArgentoApi.Utils;
using System.Net;

namespace DinoArgentoApi.Services
{
    public class RoleService
    {
        private readonly IRepository<Role> _repo;
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetAll() => await _repo.GetAll();

        public async Task<Role> GetOneById(int id)
        {
            var rol = await _repo.GetOne(x => x.Id == id);
            if (rol == null) throw new ErrorResponse(HttpStatusCode.NotFound, "Rol no encontrado");
            return rol;
        }

        public async Task<List<Role>> GetManyByIds(List<int> ids)
        {
            return await _repo.GetAll(r => ids.Contains(r.Id));
        }

        public async Task<Role> GetOneByName(string name)
        {
            var rol = await _repo.GetOne(x => x.Name == name);
            if (rol == null) throw new ErrorResponse(HttpStatusCode.NotFound, $"Rol '{name}' no encontrado");
            return rol;
        }

        public async Task<Role> CreateOne(RoleDTO dto)
        {
            var rol = _mapper.Map<Role>(dto);
            return await _repo.CreateOne(rol);
        }

        public async Task<Role> UpdateOneById(int id, UpdateRoleDTO dto)
        {
            var rol = await GetOneById(id);
            if (dto.Name != null) rol.Name = dto.Name;
            return await _repo.UpdateOne(rol);
        }

        public async Task DeleteOneById(int id)
        {
            var rol = await GetOneById(id);
            await _repo.DeleteOne(rol);
        }
    }
}
    