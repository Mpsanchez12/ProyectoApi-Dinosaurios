using AutoMapper;
using DinoArgentoApi.Models.User;
using DinoArgentoApi.Repositories;
using DinoArgentoApi.Utils;
using System.Net;

namespace DinoArgentoApi.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<User?> GetOneByUsernameOrEmail(string? email, string? username)
        {

            return await _repo.GetByUsername(username ?? email ?? "");
        }

        public async Task<User> CreateOne(User user)
        {
            return await _repo.CreateOne(user);
        }

        public async Task<User> UpdateOne(User user)
        {

            return await _repo.UpdateOne(user);
        }

        public async Task<User> GetOneById(int id)
        {

            var user = await _repo.GetOne(u => u.Id == id);
            if (user == null) throw new ErrorResponse(HttpStatusCode.NotFound, "Usuario no encontrado");
            return user;
        }


        public async Task<string[]> GeneratePwdToken(int userId, HttpRequest request)
        {

            return new[] { $"{request.Scheme}://{request.Host}/reset-password", "user@email.com" };
        }

        public async Task<bool> VerifyPwdToken(int userId, string token)
        {

            return true;
        }
    }
}
    