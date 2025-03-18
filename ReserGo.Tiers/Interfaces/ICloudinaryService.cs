
using Microsoft.AspNetCore.Http;

namespace ReserGo.Tiers.Interfaces {
    public interface ICloudinaryService {
        Task<string> GetPicture(string publicId);
        Task<string> UploadImage(IFormFile file);
    }
}
