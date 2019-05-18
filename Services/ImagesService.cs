using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Rekreatistika.Services
{
    public interface IImagesService
    {
        Task SavePicture(IFormFile picture, string folderPath, string pictureName);
    }

    public class ImagesService : IImagesService
    {

        const int max_width = 256;
        const int max_height = 256;

        public async Task SavePicture(IFormFile picture, string folderPath, string pictureName)
        {
            string fileExtension = Path.GetExtension(picture.FileName);
            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                throw new Exception("Wrong picture format!");

            if (picture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await picture.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    using (Image<Rgba32> image = Image.Load(fileBytes))
                    {
                        int width = image.Width;
                        int height = image.Height;
                        bool isInPortraitMode = height > width;
                        if (isInPortraitMode)
                        {
                            height = max_height;
                            width = Convert.ToInt32(image.Width * max_height / (double)image.Height);
                        }
                        else
                        {
                            width = max_width;
                            height = Convert.ToInt32(image.Height * max_width / (double)image.Width);
                        }
                        var filePath = folderPath + $@"/{pictureName}.jpg";
                        Directory.CreateDirectory(folderPath);
                        image.Mutate(x => x
                            .Resize(width, height));
                        image.Save(filePath); // Automatic encoder selected based on extension.

                        image.Mutate(x => x.Resize(64, Convert.ToInt32(image.Height * 64 / (double)image.Width)));
                        image.Save(folderPath + $@"/thumb_{pictureName}.jpg");
                    }
                }
            }
        }
    }
}
