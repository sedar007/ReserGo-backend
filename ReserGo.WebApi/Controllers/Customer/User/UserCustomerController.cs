using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.User;
using ReserGo.Shared.Interfaces;
using ReserGo.WebAPI.Attributes;
using ReserGo.WebAPI.Controllers.Helper;

namespace ReserGo.WebAPI.Controllers.Customer.User;

[ApiController]
[Route("api/customer/users/")]
public class UserCustomerController : ControllerBase {
    private readonly ILogger<UserCustomerController> _logger;
    private readonly ISecurity _security;
    private readonly IUserService _userService;

    public UserCustomerController(ILogger<UserCustomerController> logger, ISecurity security,
        IUserService userService) {
        _logger = logger;
        _userService = userService;
        _security = security;
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
        return await UserControllerHelper.CreateUser(request, _userService, this);
    }

    /// <summary>
    ///     Retrieve a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user object.</returns>
    /// <response code="200">User found and returned.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [ClientOnly]
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
    [ClientOnly]
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
    [ClientOnly]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Resource<UserDto>>> UpdateUser(Guid id, UserUpdateRequest request) {
        return await UserControllerHelper.UpdateUser(id, request, _userService, _security, this);
    }
}