using Microsoft.AspNetCore.Mvc;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Models;
using ReserGo.Common.Requests.User;
using ReserGo.Common.Enum;
using ReserGo.Shared.Interfaces;
using ReserGo.Shared;

namespace ReserGo.WebAPI.Controllers.Helper;

public static class UserControllerHelper {
    public static async Task<ActionResult> CreateUser(UserCreationRequest request, IUserService userService,
        ControllerBase controller, UserRole userRole = UserRole.Client) {
        try {
            var data = await userService.Create(request, userRole);

            var resource = new Resource<UserDto> {
                Data = data,
                Links = new List<Link> {
                    new() {
                        Href = controller.Url.Action("GetById", "User", new { id = data.Id }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = controller.Url.Action("UpdateUser", "User", new { id = data.Id }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return controller.Created("create", resource);
        }
        catch (InvalidDataException ex) {
            return controller.BadRequest(ex.Message);
        }
        catch (Exception ex) {
            controller.HttpContext.Items["Logger"]?.GetType().GetMethod("LogError")?.Invoke(null,
                new object[] { ex, "An error occurred while creating the user." });
            return controller.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    public static void ValidateUserAccess(Guid userId, ISecurity security) {
        // Check if the user is authenticated
        var connectedUser = security.GetCurrentUser();
        if (connectedUser == null) throw new UnauthorizedAccessException("User is not authenticated.");
        // Check if the user ID matches the connected user's ID
        if (connectedUser.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to access this user.");
    }

    public static async Task<ActionResult<Resource<UserDto>>> GetUserById(Guid userId, IUserService userService,
        ISecurity security, ControllerBase controller) {
        try {
            ValidateUserAccess(userId, security);

            var user = await userService.GetById(userId);
            if (user == null) return controller.NotFound($"User with ID {userId} not found.");

            var resource = new Resource<UserDto> {
                Data = user,
                Links = new List<Link> {
                    new() {
                        Href = controller.Url.Action("GetById", "User", new { userId }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new() {
                        Href = controller.Url.Action("UpdateUser", "User", new { userId }),
                        Rel = "update",
                        Method = "PUT"
                    }
                }
            };

            return controller.Ok(resource);
        }
        catch (UnauthorizedAccessException ex) {
            return controller.Forbid();
        }
        catch (InvalidDataException ex) {
            return controller.NotFound(ex.Message);
        }
        catch (Exception ex) {
            controller.HttpContext.Items["Logger"]?.GetType().GetMethod("LogError")?.Invoke(null,
                new object[] { ex, "An error occurred while retrieving the user." });
            return controller.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    public static async Task<ActionResult> DeleteUser(Guid userId, IUserService userService, ISecurity security,
        ILogger logger, ControllerBase controller) {
        try {
            ValidateUserAccess(userId, security);
            await userService.Delete(userId);
            controller.Response.Cookies.Delete(Consts.AuthToken);
            return controller.NoContent();
        }
        catch (UnauthorizedAccessException ex) {
            return controller.Forbid(ex.Message);
        }
        catch (InvalidDataException ex) {
            return controller.NotFound(ex.Message);
        }
        catch (Exception ex) {
            logger.LogError(ex, "An error occurred while deleting the user.");
            return controller.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    public static async Task<ActionResult<Resource<UserDto>>> UpdateUser(Guid userId, UserUpdateRequest request,
        IUserService userService, ISecurity security, ControllerBase controller) {
        try {
            ValidateUserAccess(userId, security);
            var updatedUser = await userService.UpdateUser(userId, request);

            var resource = new Resource<UserDto> {
                Data = updatedUser,
                Links = new List<Link> {
                    new() {
                        Href = controller.Url.Action("GetById", "User", new { userId }),
                        Rel = "self",
                        Method = "GET"
                    }
                }
            };

            return controller.Ok(resource);
        }
        catch (UnauthorizedAccessException ex) {
            return controller.Forbid(ex.Message);
        }
        catch (InvalidDataException ex) {
            return controller.BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return controller.NotFound(ex.Message);
        }
    }
}