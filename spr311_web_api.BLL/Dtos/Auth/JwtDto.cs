namespace spr311_web_api.BLL.Dtos.Auth
{
    public class JwtDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
