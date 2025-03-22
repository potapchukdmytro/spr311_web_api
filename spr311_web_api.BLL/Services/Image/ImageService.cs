using Microsoft.AspNetCore.Http;

namespace spr311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        public async Task<string?> SaveImageAsync(IFormFile image, string directory)
        {
            if (Settings.ImagesPath == null)
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
                }

                return imageName;
            }

            return null;
        }
    }
}
