namespace spr311_web_api.BLL.Services.EmailService
{
    public interface IEmailService
    {
        Task SendMessageAsync(string to, string subject, string body, bool isHtml = false);
    }
}
