using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile File)
        {
            // Validate the file
            if (File == null || File.Length == 0)
                throw new ArgumentException("No File uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var extension = Path.GetExtension(File.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only JPG, JPEG, and PNG files are allowed.");

            if (File.Length > 5 * 1024 * 1024)
                throw new ArgumentException("File size exceeds the 5MB limit.");

            // Save the file
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/products/");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
            var filePath = Path.Combine(uploadsFolder, FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }
            return $"/images/products/{FileName}";
        }
    }
}
