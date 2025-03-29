using System.ComponentModel.DataAnnotations;

namespace spr311_web_api.BLL.Dtos.Account
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
