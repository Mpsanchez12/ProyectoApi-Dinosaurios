using DinoArgentoApi.Models.User;
using Microsoft.AspNetCore.Identity;

namespace DinoArgentoApi.Services
{
    public interface IEncoderService { string Encrypt(string password); bool Verify(string hashed, string input); }

    public class EncoderService : IEncoderService
    {
        private readonly PasswordHasher<User> _hasher = new();
        public string Encrypt(string password) => _hasher.HashPassword(new User(), password);
        public bool Verify(string hashed, string input) =>
            _hasher.VerifyHashedPassword(new User(), hashed, input) == PasswordVerificationResult.Success;
    }
}