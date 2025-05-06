using Microsoft.AspNetCore.Mvc;

using ReserGo.Business.Interfaces;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;
using ReserGo.Common.Security;
using ReserGo.WebAPI.Attributes;
using ReserGo.Common.Models;
namespace ReserGo.WebAPI.Controllers.Administration.User;
   
[ApiController]
[Route("api/administration/users/")]
public class UserController : ControllerBase {
    
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly ISecurity _security;

    public UserController(ILogger<UserController> logger, ISecurity security, IUserService userService) {
        _logger = logger;
        _userService = userService;
        _security = security;
    }
    
    /// <summary>
    /// Create a new user.
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
        try {
            UserDto data = await _userService.Create(request);

            var resource = new Resource<UserDto> {
                Data = data,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id = data.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(UpdateUser), new { id = data.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Created("create", resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while creating the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Retrieve a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user object.</returns>
    /// <response code="200">User found and returned.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [AdminOnly]
    [HttpGet("getById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Resource<UserDto>>> GetById(int id) {
        try {
            var user = await _userService.GetById(id);
            if (user == null) {
                return NotFound($"User with ID {id} not found.");
            }

            var resource = new Resource<UserDto> {
                Data = user,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
                        Href = Url.Action(nameof(UpdateUser), new { id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Remove a user by their ID.
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
    public async Task<ActionResult> Delete(int id) {
        try {
            await _userService.Delete(id);
            return NoContent();
        }
        catch (InvalidDataException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
    /// Update an existing user.
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
    public async Task<ActionResult<Resource<UserDto>>> UpdateUser(int id, UserUpdateRequest request) {
        try {
            var updatedUser = await _userService.UpdateUser(id, request);

            var resource = new Resource<UserDto> {
                Data = updatedUser,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetById), new { id }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return Ok(resource);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return NotFound(ex.Message);
        }
    }
    
    /// <summary>
    /// Retrieve the information of the connected user.
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
            ConnectedUser? connectedUser = _security.GetCurrentUser();
            if (connectedUser == null) {
                return Unauthorized("User is not authenticated.");
            }

            UserDto? user = await _userService.GetById(connectedUser.UserId);
            if (user == null) {
                return Unauthorized("This user does not exist.");
            }

            var resource = new Resource<UserDto> {
                Data = user,
                Links = new List<Link> {
                    new Link {
                        Href = Url.Action(nameof(GetConnectedUser)),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link {
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
    /// Retrieve the profile picture of the connected user.
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

            ConnectedUser? connectedUser = _security.GetCurrentUser();
            if (connectedUser is null) {
                string errorMessage = "User is not authenticated.";
                _logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }
            
            string profilePicture = await _userService.GetProfilePicture(connectedUser.UserId);
            _logger.LogInformation("Profile picture of user {id} retrieved successfully", connectedUser.UserId);
            return Ok(profilePicture);
        } catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the profile picture of the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
    
    /// <summary>
/// Update the profile picture of the connected user.
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

        ConnectedUser? connectedUser = _security.GetCurrentUser();
        if (connectedUser is null) {
            string errorMessage = "User is not authenticated.";
            _logger.LogError(errorMessage);
            return Unauthorized(errorMessage);
        }

        if (file == null || file.Length == 0) {
            _logger.LogWarning("No file sent for upload.");
            return BadRequest("No file sent.");
        }

        string? newProfilePictureUrl = await _userService.UpdateProfilePicture(connectedUser.UserId, file);
        if (string.IsNullOrEmpty(newProfilePictureUrl)) {
            _logger.LogWarning("Profile picture update failed for user ID: {UserId}", connectedUser.UserId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Profile picture update failed.");
        }

        _logger.LogInformation("Profile picture of user {id} updated successfully", connectedUser.UserId);
        return Ok(newProfilePictureUrl);
    } catch (Exception ex) {
        _logger.LogError(ex, "An error occurred while updating the profile picture of the connected user.");
        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
    }
}
}

