using ReserGo.Business.Interfaces;
using ReserGo.Shared.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ReserGo.WebAPI.Controllers {

    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase {

        private readonly IImageService _imageService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger, IImageService imageService) {
            _logger = logger;
            _imageService = imageService;
        }

        /// <summary>
        /// Retrieves the image URL for the given public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the image.</param>
        /// <returns>The URL of the image.</returns>
        [HttpGet("{publicId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(string publicId)
        {
            try {
                string imageUrl = await _imageService.GetPicture(publicId);
                if (string.IsNullOrEmpty(imageUrl)) {
                    _logger.LogWarning("Image not found for publicId: {PublicId}", publicId);
                    return NotFound(new { message = "Image not found" });
                }
                return Ok(new { Url = imageUrl });
            } catch (Exception e) {
                _logger.LogError(e, "Error retrieving image for publicId: {PublicId}", publicId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        /// <summary>
        /// Uploads an image and returns its URL.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <param name="userId">The ID of the user uploading the image.</param>
        /// <returns>The URL of the uploaded image.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage(IFormFile? file, int userId)
        {
            if (file == null || file.Length == 0) {
                _logger.LogWarning("No file sent for upload.");
                return BadRequest("No file sent.");
            }

            try {
                string? uploadResult = await _imageService.UploadImage(file, userId);
                if (string.IsNullOrEmpty(uploadResult)) {
                    _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Image upload failed.");
                }
                return Ok(new { publicId = uploadResult });
            } catch (Exception e) {
                _logger.LogError(e, "Error uploading image for file: {FileName}", file.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }
    }
}