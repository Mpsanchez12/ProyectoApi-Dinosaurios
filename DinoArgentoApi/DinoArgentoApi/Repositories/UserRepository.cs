using DinoArgentoApi.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DinoArgentoApi.Config;


namespace DinoArgentoApi.Repositories
{
   
        public interface IUserRepository
        {
            Task<User?> GetByUsername(string username);
            Task<User> CreateOne(User user);
            Task<User> UpdateOne(User user);
            Task<User?> GetOne(Expression<Func<User, bool>> filter);
        }

        public class UserRepository : IUserRepository
        {
            private readonly AppDbContext _context;
            public UserRepository(AppDbContext context) { _context = context; }

            public async Task<User?> GetByUsername(string username) =>
                await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == username);

            public async Task<User> CreateOne(User user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<User> UpdateOne(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }

            public async Task<User?> GetOne(Expression<Func<User, bool>> filter)
            {
                return await _context.Users.FirstOrDefaultAsync(filter);
            }
        }
    }


