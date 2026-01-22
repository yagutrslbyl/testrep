using System.Threading.Tasks;

namespace Eat.Utilities
{
    public static class UploadImage
    {
        public static async Task<string> SaveImage(this IFormFile file, IWebHostEnvironment _env, string path)
        {
            if (file.ContentType.Contains("image/"))
            {

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (file.Length > 2 * 1024 * 1024)
                {
                    return ("Image boyukdur");
                }

                string fileName = Guid.NewGuid() + "_" + file.FileName;
                string fullPath = Path.Combine(_env.WebRootPath, path, fileName);
                using (var steam = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(steam);
                }

                return fileName;
            }
            else
            {
                return ("Image yukleyin");
            }
           
        }
    }
}
