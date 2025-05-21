using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using ReserGo.Business.Interfaces;
using ReserGo.Business.Validator;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Enum;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.User;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class UserService : IUserService {
    private readonly ILogger<UserService> _logger;
    private readonly ILoginService _loginService;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IImageService _imageService;
    private readonly IMemoryCache _cache;

    public UserService(ILogger<UserService> logger, IUserDataAccess userDataAccess,
        ILoginService loginService, IImageService imageService, IMemoryCache cache) {
        _logger = logger;
        _loginService = loginService;
        _userDataAccess = userDataAccess;
        _imageService = imageService;
        _cache = cache;
    }

    public async Task<UserDto> Create(UserCreationRequest request, UserRole role) {
        try {
            var userByUsername = await _userDataAccess.GetByUsername(request.Username);
            if (userByUsername is not null) {
                var errorMessage = "This username is already in use.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var userByEmail = await _userDataAccess.GetByEmail(request.Email);
            if (userByEmail is not null) {
                var errorMessage = "This email address is already in use.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var error = UserValidator.GetErrorCreationRequest(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            var newUser = new User {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Username = request.Username,
                Role = role
            };

            newUser = await _userDataAccess.Create(newUser);
            await _loginService.Create(request.Password, newUser);

            _logger.LogInformation("User { id } created", newUser.Id);
            return newUser.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<UserDto?> GetById(Guid id) {
        try {
            var cacheKey = $"GetById_{id}";

            if (_cache.TryGetValue(cacheKey, out UserDto? cachedUser)) {
                _logger.LogInformation("Returning cached user for ID: {Id}", id);
                return cachedUser;
            }

            var user = await _userDataAccess.GetById(id);
            if (user is null) {
                var errorMessage = "This user does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("User { id } retrieved successfully", user.Id);
            var userDto = user.ToDto();
            _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));
            return userDto;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<string> GetProfilePicture(Guid userId) {
        var user = await GetById(userId);
        if (user is null) {
            var errorMessage = "This user does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var publicId = user.ProfilePicture ?? Consts.DefaultProfile;

        _logger.LogInformation("Getting picture with publicId: {PublicId}", publicId);
        var url = await _imageService.GetPicture(publicId);
        _logger.LogInformation("Retrieved picture URL: {Url}", url);
        return url;
    }


    public async Task<UserDto?> GetByEmail(string email) {
        try {
            var cacheKey = $"GetByEmail_{email}";

            if (_cache.TryGetValue(cacheKey, out UserDto? cachedUser)) {
                _logger.LogInformation("Returning cached user for email: {Email}", email);
                return cachedUser;
            }

            var user = await _userDataAccess.GetByEmail(email);
            if (user is null) {
                var errorMessage = "This user does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("User {id} retrieved successfully", user.Id);
            var userDto = user.ToDto();
            _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return userDto;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<UserDto?> GetByUsername(string username) {
        try {
            var cacheKey = $"GetByUsername_{username}";

            if (_cache.TryGetValue(cacheKey, out UserDto? cachedUser)) {
                _logger.LogInformation("Returning cached user for username: {Username}", username);
                return cachedUser;
            }

            var user = await _userDataAccess.GetByUsername(username);
            if (user is null) {
                var errorMessage = "This user does not exist.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            _logger.LogInformation("User { id } retrieved successfully", user.Id);

            var userDto = user.ToDto();
            _cache.Set(cacheKey, userDto, TimeSpan.FromMinutes(Consts.CacheDurationMinutes));

            return userDto;
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task Delete(Guid id) {
        try {
            var user = await _userDataAccess.GetById(id);
            if (user is null) {
                var errorMessage = "User not found";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var userId = user.Id;
            var email = user.Email;
            var username = user.Username;
            var publicId = user.ProfilePicture;
            await _userDataAccess.Delete(user);
            if (publicId is not null) {
                var deleteResult = await _imageService.DeleteImage(publicId);
                if (!deleteResult) _logger.LogWarning("Failed to delete image with publicId: {PublicId}", publicId);
            }

            RemoveCache(id, email, username);
            _logger.LogInformation("User { id } deleted successfully", userId);
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<UserDto> UpdateUser(Guid id, UserUpdateRequest request) {
        try {
            var user = await _userDataAccess.GetById(id);
            if (user is null) throw new Exception("User not found");

            var existingEmailUser = await _userDataAccess.GetByEmail(request.Email);
            if (existingEmailUser is not null && existingEmailUser.Id != id) {
                var errorMessage = "This email address is already in use.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var existingUsernameUser = await _userDataAccess.GetByUsername(request.Username);
            if (existingUsernameUser is not null && existingUsernameUser.Id != id) {
                var errorMessage = "This username is already in use.";
                _logger.LogError(errorMessage);
                throw new InvalidDataException(errorMessage);
            }

            var error = UserValidator.GetErrorUpdateRequest(request);
            if (string.IsNullOrEmpty(error) == false) {
                _logger.LogError(error);
                throw new InvalidDataException(error);
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Username = request.Username;
            user.Bio = request.Bio;
            user.PhoneNumber = request.PhoneNumber;
            if (user.Address == null) {
                user.Address = new Address {
                    Street = request.Address?.Street,
                    City = request.Address?.City,
                    State = request.Address?.State,
                    PostalCode = request.Address?.PostalCode,
                    Country = request.Address?.Country
                };
            }
            else {
                user.Address.Street = request.Address?.Street;
                user.Address.City = request.Address?.City;
                user.Address.State = request.Address?.State;
                user.Address.PostalCode = request.Address?.PostalCode;
                user.Address.Country = request.Address?.Country;
            }

            _logger.LogInformation("User { id } updated successfully", user.Id);
            await _userDataAccess.Update(user);
            RemoveCache(user.Id, user.Email, user.Username);
            return user.ToDto();
        }
        catch (Exception e) {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<string> UpdateProfilePicture(Guid userId, IFormFile file) {
        _logger.LogInformation("Uploading image with file name: {FileName}", file.FileName);

        var user = await _userDataAccess.GetById(userId);
        if (user is null) {
            var errorMessage = "This user does not exist.";
            _logger.LogError(errorMessage);
            throw new InvalidDataException(errorMessage);
        }

        var oldPublicId = user.ProfilePicture;

        var publicId = await _imageService.UploadImage(file, userId);
        if (string.IsNullOrEmpty(publicId)) {
            _logger.LogWarning("Image upload failed for file: {FileName}", file.FileName);
            throw new InvalidDataException("Image upload failed.");
        }

        if (oldPublicId is not null) {
            var deleteResult = await _imageService.DeleteImage(oldPublicId);
            if (!deleteResult) _logger.LogWarning("Failed to delete old image with publicId: {PublicId}", oldPublicId);
        }

        user.ProfilePicture = publicId;
        await _userDataAccess.Update(user);

        _logger.LogInformation("Image uploaded successfully. URL: {Url}", publicId);
        RemoveCache(userId, user.Email, user.Username);
        return publicId;
    }

    private void RemoveCache(Guid id, string email, string username) {
        _cache.Remove($"GetById_{id}");
        _cache.Remove($"GetByEmail_{email}");
        _cache.Remove($"GetByUsername_{username}");
    }
}