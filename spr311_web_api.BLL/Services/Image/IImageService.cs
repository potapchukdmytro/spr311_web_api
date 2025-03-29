using Microsoft.AspNetCore.Http;

namespace spr311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string filePath);
        void DeleteImage(string directory);
        Task<List<string>> SaveProductImagesAsync(List<IFormFile> images, string path);
    }
}
