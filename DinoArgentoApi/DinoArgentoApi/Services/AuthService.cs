using AutoMapper;
using DinoArgentoApi.Models.Role;
using DinoArgentoApi.Models.User;
using DinoArgentoApi.Models.User.Dto;
using DinoArgentoApi.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DinoArgentoApi.Services
{
   
        public interface IAuthService
        {
            Task<LoginResponse> Login(LoginDTO login, HttpContext context);
            Task Logout(HttpContext context);
            Task<UserDTO> Register(RegisterDTO register);
            Task<UserDTO> UpdateRolesToUser(int userId, List<int> roleIds);
            Task GeneratePwdTokenToUser(HttpContext context);
            Task VerifyUserPwdToken(int userId, string token);
        Task UpdateRolesToUser(int userId, object roleIds);
    }

        public class AuthService : IAuthService
        {
            private readonly UserService _userService;
            private readonly RoleService _roleService;
            private readonly EncoderService _encoder;
            private readonly EmailService _emailService;
            private readonly IMapper _mapper;
            private readonly string _secret;

            public AuthService(UserService userService, IMapper mapper, IConfiguration config,
                               RoleService roleService, EncoderService encoder, EmailService emailService)
            {
                _userService = userService;
                _mapper = mapper;
                _secret = config.GetSection("Secrets:jwt")?.Value?.ToString()
                          ?? throw new Exception("invalid jwt secret");
                _roleService = roleService;
                _encoder = encoder;
                _emailService = emailService;
            }

            public async Task GeneratePwdTokenToUser(HttpContext context)
            {
                int userId = GetUserIdFromContext(context);
                string[] data = await _userService.GeneratePwdToken(userId, context.Request);
                await _emailService.SendResetPwdAsync(data[1], data[0]);
            }

            public async Task VerifyUserPwdToken(int userId, string token)
            {
                bool isCorrect = await _userService.VerifyPwdToken(userId, token);
                if (!isCorrect) throw new ErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
            }

        public async Task<LoginResponse> Login(LoginDTO login, HttpContext context)
        {
            User? user = await _userService.GetOneByUsernameOrEmail(login.UserName, login.UserName);

            if (user == null || !_encoder.Verify(user.Password, login.Password))
                throw new ErrorResponse(HttpStatusCode.BadRequest, "Invalid Credentials");

          

            return new LoginResponse()
            {
                Token = GenerateJwtToken(user),
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task Logout(HttpContext context)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            public async Task SetCookie(User user, HttpContext context)
            {
                var claims = new List<Claim> { new Claim("id", user.Id.ToString()) };
                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddDays(7) });
            }

            public async Task<UserDTO> Register(RegisterDTO register)
            {
                User? u = await _userService.GetOneByUsernameOrEmail(register.Email, register.UserName);
                if (u != null) throw new ErrorResponse(HttpStatusCode.BadRequest, "User already exists");

                var user = _mapper.Map<User>(register);
                user.Password = _encoder.Encrypt(user.Password);

                var role = await _roleService.GetOneByName("User");
                user.Roles.Add(role);

                var created = await _userService.CreateOne(user);
                return _mapper.Map<UserDTO>(created);
            }

            public string GenerateJwtToken(User user)
            {
                var claims = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) });
                if (user.Roles != null)
                {
                    foreach (var role in user.Roles)
                        claims.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                }

                var key = Encoding.UTF8.GetBytes(_secret);
                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = credentials
                };

                return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
            }

            public async Task<UserDTO> UpdateRolesToUser(int userId, List<int> roleIds)
            {
                User user = await _userService.GetOneById(userId);
                List<Role> roles = await _roleService.GetManyByIds(roleIds);
                user.Roles = roles;
                var updatedUser = await _userService.UpdateOne(user);
                return _mapper.Map<UserDTO>(updatedUser);
            }

            private int GetUserIdFromContext(HttpContext context)
            {
                var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                if (idClaim == null || !int.TryParse(idClaim, out int id))
                    throw new ErrorResponse(HttpStatusCode.BadRequest, "invalid jwt token");
                return id;
            }

        public Task UpdateRolesToUser(int userId, object roleIds)
        {
            throw new NotImplementedException();
        }
    }
    }

