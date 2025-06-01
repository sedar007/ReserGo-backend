using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Shared.Interfaces;

namespace ReserGo.WebAPI.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;
    private readonly ILogger<ImageController> _logger;
    private readonly ISecurity _security;

    public ImageController(ILogger<ImageController> logger, IImageService imageService, ISecurity security) {
        _logger = logger;
        _imageService = imageService;
        _security = security;
    }

    /// <summary>
    ///     Retrieves the image URL for the given public ID.
    /// </summary>
    /// <param name="publicId">The public ID of the image.</param>
    /// <returns>The URL of the image.</returns>
    [HttpGet("{publicId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImage(string publicId) {
        try {
            var imageUrl = await _imageService.GetPicture(publicId);
            if (string.IsNullOrEmpty(imageUrl)) {
                _logger.LogWarning("Image not found for publicId: {PublicId}", publicId);
                return NotFound(new { message = "Image not found" });
            }

            return Ok(new { Url = imageUrl });
        }
        catch (Exception e) {
            _logger.LogError(e, "Error retrieving image for publicId: {PublicId}", publicId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    /// <summary>
    ///     Uploads an image and returns its URL.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    /// <param name="userId">The ID of the user uploading the image.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage(IFormFile? file, Guid userId) {
        if (file == null || file.Length == 0) {
            _logger.LogWarning("No file sent for upload.");
            return BadRequest("No file sent.");
        }

        try {
            var uploadResult = await _imageService.UploadImage(file, userId);
            if (string.IsNullOrEmpty(uploadResult)) {
                _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError, "Image upload failed.");
            }

            return Ok(new { url = uploadResult });
        }
        catch (Exception e) {
            _logger.LogError(e, "Error uploading image for file: {FileName}", file.FileName);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    /// <summary>
    ///     Uploads an image and returns its URL.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    /// <returns>The URL of the uploaded image.</returns>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage(IFormFile? file) {
        if (file == null || file.Length == 0) {
            _logger.LogWarning("No file sent for upload.");
            return BadRequest("No file sent.");
        }

        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        if (!allowedContentTypes.Contains(file.ContentType.ToLower()) ||
            !allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) {
            _logger.LogWarning("Invalid file type: {FileName}", file.FileName);
            return BadRequest("Only image files (JPEG, PNG, GIF) are allowed.");
        }


        try {
            var currentUser = _security.GetCurrentUser();
            if (currentUser == null) {
                _logger.LogWarning("Unauthorized access attempt.");
                return Unauthorized("User not authenticated.");
            }

            var uploadResult = await _imageService.UploadImage(file, currentUser.UserId);
            if (string.IsNullOrEmpty(uploadResult)) {
                _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError, "Image upload failed.");
            }

            return Ok(new { url = uploadResult });
        }
        catch (Exception e) {
            _logger.LogError(e, "Error uploading image for file: {FileName}", file.FileName);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }
}