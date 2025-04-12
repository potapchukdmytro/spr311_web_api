using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace spr311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;

        public ImageService(ILogger<ImageService> logger)
        {
            _logger = logger;
        }

        public void DeleteImage(string filePath)
        {
            if (string.IsNullOrEmpty(Settings.ImagesPath))
            {
                return;
            }

            string workPath = Path.Combine(Settings.ImagesPath, filePath);

            if (File.Exists(workPath))
            {
                File.Delete(workPath);
            }
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string directory)
        {
            if (string.IsNullOrEmpty(Settings.ImagesPath))
            {
                return null;
            }

            var types = image.ContentType.Split('/');

            if (types[0] == "image")
            {
                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string workPath = Path.Combine(Settings.ImagesPath, directory);
                string filePath = Path.Combine(workPath, imageName);

                if (!Directory.Exists(workPath))
                {
                    Directory.CreateDirectory(workPath);
                }

                using (var stream = File.Create(filePath))
                {
                    await image.CopyToAsync(stream);
                    _logger.LogInformation($"Image saved: {filePath}; {DateTime.Now}");
                }

                return imageName;
            }

            return null;
        }

        public async Task<List<string>> SaveProductImagesAsync(List<IFormFile> images, string path)
        {
            List<string> imagesName = new List<string>();

            foreach (var image in images)
            {
                string? imageName = await SaveImageAsync(image, path);
                if(imageName != null)
                {
                    imagesName.Add(imageName);
                }
            }

            return imagesName;
        }
    }
}
