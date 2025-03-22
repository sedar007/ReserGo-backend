using Microsoft.AspNetCore.Mvc;

using ReserGo.Business.Interfaces;
using ReserGo.Shared.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;
using ReserGo.Common.Security;

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
            return Created("create", data);
        }
        catch (InvalidDataException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the user.");
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
    [HttpGet("getById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto?>> GetById(int id) {
        try {
            var user = await _userService.GetById(id);
            return Ok(user);
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
    public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpdateRequest request)
    {
        try {
            var updatedUser = await _userService.UpdateUser(id, request);
            return Ok(updatedUser);
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
    public async Task<ActionResult<UserDto?>> GetConnectedUser() {
        try {
            _logger.LogInformation("Attempting to retrieve the connected user.");

            ConnectedUser? connectedUser = _security.GetCurrentUser();
            if (connectedUser is null) {
                string errorMessage = "User is not authenticated.";
                _logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }

            UserDto? user = await _userService.GetById(connectedUser.UserId);
            if (user is null) {
                string errorMessage = "This user does not exist.";
                _logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }

            _logger.LogInformation("User {id} retrieved successfully", user.Id);
            return Ok(user);
        } catch (Exception ex) {
            _logger.LogError(ex, "An error occurred while retrieving the connected user.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}

