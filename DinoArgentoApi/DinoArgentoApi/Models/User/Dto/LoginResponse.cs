namespace DinoArgentoApi.Models.User.Dto
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
    }
}
