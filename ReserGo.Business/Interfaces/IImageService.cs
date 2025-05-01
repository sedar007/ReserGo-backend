using Microsoft.AspNetCore.Http;
namespace ReserGo.Business.Interfaces {
    public interface IImageService{
        Task<string> GetPicture(string publicId);
        Task<string?> UploadImage(IFormFile file, int userId);
        Task<bool> DeleteImage(string publicId);
    }
}
