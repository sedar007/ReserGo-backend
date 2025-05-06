using Microsoft.AspNetCore.Http;

namespace ReserGo.Tiers.Interfaces;

public interface ICloudinaryService {
    string GetPicture(string publicId);
    Task<string?> UploadImage(IFormFile file, int userId);
    Task<bool> DeleteImage(string publicId);
}