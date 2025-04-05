namespace spr311_web_api.BLL.Configuration
{
    public class JwtSettings
    {
        public string? SecretKey { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int ExpMinutes { get; set; }
    }
}
