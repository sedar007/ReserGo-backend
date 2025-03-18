using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Interfaces;

namespace ReserGo.Business.Implementations;

public class LoginService : ILoginService {
    
    private readonly ISecurity _security;
    private readonly ILogger<UserService> _logger;
    private readonly ILoginDataAccess _loginDataAccess;
	
    public LoginService(ILogger<UserService> logger, ISecurity security, ILoginDataAccess loginDataAccess) {
        _logger = logger;
        _security = security;
        _loginDataAccess = loginDataAccess;
    }
    
    /*
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
    }*/
    
    public async Task<LoginDto?> Create(string password, User user) {
        
         // User validation
         if (user == null || user.Id <= 0) { 
            _logger.LogError("Invalid user object");
            throw new InvalidDataException("User must be created before login");
         }
         
         // Hash passwd
         var hashedPassword = _security.HashPassword(password);
         if (string.IsNullOrEmpty(hashedPassword)) {
             _logger.LogError("Password could not be hashed for user: {Username}", user.Username);
             throw new InvalidDataException("Password could not be hashed");
         }
         
         var login = new Login {
             UserId = user.Id,  
             Password = hashedPassword,
             Username = user.Username,
             LastLogin = null,
             FailedAttempts = 0,
             IsLocked = false
         };
    
         var createdLogin = await _loginDataAccess.Create(login);
         if (createdLogin == null) {
             string errorMessage = "Login could not be created";
             _logger.LogError(errorMessage);
             throw new InvalidDataException(errorMessage);             
         }
         
         _logger.LogInformation("User {Username} created successfully", user.Username);
         return createdLogin.ToDto();
    }
    
}