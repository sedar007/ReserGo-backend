using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Enum;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.User;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Helper;

namespace ReserGo.WebAPI.Controllers.Administration.User;

[ApiController]
[Route("api/administration/users/")]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;
    private readonly ISecurity _security;
    private readonly UserRole _userRole;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, ISecurity security, IUserService userService) {
        _logger = logger;
        _userService = userService;
        _security = security;
        _userRole = UserRole.Admin;
    }

    /// <summary>
    ///     Create a new user.
    /// </summary>
    /// <param name="request">The user creation request containing necessary information.</param>
    /// <returns>The created user object.</returns>
    /// <response code="201">User created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Create(UserCreationRequest request) {
        return await UserControllerHelper.CreateUser(request, _userService, this, _userRole);
    }

    /// <summary>
    ///     Retrieve a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user object.</returns>
    /// <response code="200">User found and returned.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [AdminOnly]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<UserDto>>> GetById(Guid id) {
        return await UserControllerHelper.GetUserById(id, _userService, _security, this);
    }

    /// <summary>
    ///     Remove a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">User removed successfully.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [AdminOnly]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id) {
        return await UserControllerHelper.DeleteUser(id, _userService, _security, _logger, this);
    }

    /// <summary>
    ///     Update an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The user update request.</param>
    /// <returns>The updated user object.</returns>
    /// <response code="200">User updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">User not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<UserDto>>> UpdateUser(Guid id, UserUpdateRequest request) {
        return await UserControllerHelper.UpdateUser(id, request, _userService, _security, this);
    }

    /// <summary>
    ///     Retrieve the information of the connected user.
    /// </summary>
    /// <returns>The connected user object.</returns>
    /// <response code="200">User information retrieved successfully.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("getConnectedUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<UserDto>>> GetConnectedUser() {
        try {
            var connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) return Unauthorized("User is not authenticated.");

            var user = await _userService.GetById(connectedUser.UserId);
            if (user == null) return Unauthorized("This user does not exist.");

            var resource = new Resource<UserDto> {
                Data = user,
                Links = new List<Link> {
                    new() {
                        Href = Url.Action(nameof(GetConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = Url.Action(nameof(UpdateUser), new { id = user.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Retrieve the profile picture of the connected user.
    /// </summary>
    /// <returns>The profile picture URL of the connected user.</returns>
    /// <response code="200">Profile picture retrieved successfully.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpGet("ProfilePicture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string?>> GetConnectedUserProfilePicture() {
        try {
            _logger.LogInformation("Attempting to retrieve the profile picture of the connected user.");

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser is null) {
                var errorMessage = "User is not authenticated.";
                _logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }

            var profilePicture = await _userService.GetProfilePicture(connectedUser.UserId);
            _logger.LogInformation("Profile picture of user {id} retrieved successfully", connectedUser.UserId);
            return Ok(profilePicture);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the profile picture of the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Update the profile picture of the connected user.
    /// </summary>
    /// <param name="file">The new profile picture file.</param>
    /// <returns>The URL of the updated profile picture.</returns>
    /// <response code="200">Profile picture updated successfully.</response>
    /// <response code="400">Invalid file or request data.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [HttpPut("UpdateProfilePicture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string?>> UpdateProfilePicture(IFormFile? file) {
        try {
            _logger.LogInformation("Attempting to update the profile picture of the connected user.");

            var connectedUser = _security.GetCurrentUser();
            if (connectedUser is null) {
                var errorMessage = "User is not authenticated.";
                _logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }

            if (file == null || file.Length == 0) {
                _logger.LogWarning("No file sent for upload.");
                return BadRequest("No file sent.");
            }

            var newProfilePictureUrl = await _userService.UpdateProfilePicture(connectedUser.UserId, file);
            if (string.IsNullOrEmpty(newProfilePictureUrl)) {
                _logger.LogWarning("Profile picture update failed for user ID: {UserId}", connectedUser.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Profile picture update failed.");
            }

            _logger.LogInformation("Profile picture of user {id} updated successfully", connectedUser.UserId);
            return Ok(newProfilePictureUrl);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while updating the profile picture of the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}