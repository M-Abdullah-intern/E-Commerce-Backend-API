namespace ECommerceAPI.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile File);
    }
}
