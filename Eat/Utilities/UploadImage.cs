using System.Threading.Tasks;

namespace Eat.Utilities
{
    public static class UploadImage
    {
        public static async Task<string> SaveImage(this IFormFile file, IWebHostEnvironment _env, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Guid.NewGuid() + "_" + file.FileName;
            string fullName = Path.Combine(_env.WebRootPath, path, fileName);

            using (var stream = new FileStream(fullName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
    }
