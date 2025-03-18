/*using ReserGo.Business.Interfaces;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Requests.Security;
using ReserGo.Common.Entity;
using Microsoft.Extensions.Logging;
using ReserGo.Common.DTO;
using ReserGo.Shared;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations
{
    public class AuthService : IAuthService {
        private readonly ILogger<AuthService> _logger;
        private readonly ISecurity _security;

        private readonly IUserDataAccess _userDataAccess;
        private readonly ILoginService _loginService;

        public UserService(ILogger<UserService> logger, IUserDataAccess dataAccess, IAuthService authService) {

        public AuthService(ILogger<AuthService> logger, IUserDataAccess userDataAccess, ISecurity security, ILoginService loginService) {
            _logger = logger;
            _security = security;
            _userDataAccess = userDataAccess;
            _loginService = loginService;
        }
        
        private async Task<User?> GetUser(LoginRequest request) {
            if(Utils.CheckMail(request.Login))
                return await _userDataAccess.GetByEmail(request.Login);
            return await _userDataAccess.GetByUsername(request.Login);
        }

        public async Task<AuthenticateResponse?> Login(LoginRequest? request) {
            ArgumentNullException.ThrowIfNull(request);

            User? user = await GetUser(request);

            if (user == null) {
                _logger.LogWarning("User not Found");
                throw new KeyNotFoundException($"The user: {request.Login} or the password is Incorrect");
            }
            
            Login? login = await _loginService.GetByUserId(user.Id);
            
            if (login == null) {
                _logger.LogWarning("User not Found");
                throw new KeyNotFoundException($"The user: {request.Login} or the password is Incorrect");
            }
            
            if (!_security.VerifyPassword(login.Password, request.Password)) {
                _logger.LogWarning("Invalid Password");
                throw new KeyNotFoundException($"The user: {request.Login} or the password is Incorrect");
            }
            return new AuthenticateResponse(user, _security.GenerateJwtToken(user.Username, user.Id, user.Role), user.Role);
        }
        
        public async Task<LoginDto?> Create(string password, User user) {
            ValidateUser(user);
            var hashedPassword = HashPassword(password, user);
            return await CreateLogin(user, hashedPassword);
        }
        
        private void ValidateUser(User user) {
            if (user == null || user.Id <= 0) { 
                _logger.LogError("Invalid user object");
                throw new InvalidDataException("User must be created before login");
            }
        }
        
        private string HashPassword(string password, User user) {
            var hashedPassword = _security.HashPassword(password);
            if (string.IsNullOrEmpty(hashedPassword)) {
                _logger.LogError("Password could not be hashed for user: {Username}", user.Username);
                throw new InvalidDataException("Password could not be hashed");
            }
            return hashedPassword;
        }
        
        private async Task<LoginDto?> CreateLogin(User user, string hashedPassword) {
            Login login = new Login {
                UserId = 
                    user.Id
                ,
                Password = hashedPassword,
                Username = user.Username,
                LastLogin = null,
                FailedAttempts = 0,
                IsLocked = false
            };
    
            var createdLogin = await _loginService.Create(login);
            if (createdLogin == null) {
                throw new InvalidDataException("Login could not be created");
            }
            _logger.LogInformation("User {Username} created successfully", user.Username);
            return createdLogin.ToDto();
        }
    }
}*/
