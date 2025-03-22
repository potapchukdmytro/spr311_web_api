using Microsoft.AspNetCore.Http;

namespace spr311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directory);
    }
}
