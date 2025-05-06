using ReserGo.Common.Requests.User;

namespace ReserGo.Business.Validator;

public static class UserValidator {
    public static string GetErrorCreationRequest(UserCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName)) return "FirstName or LastName cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Username)) return "Username cannot be empty.";
        string emailError = EmailValidator.GetError(request.Email);
        if (!string.IsNullOrEmpty(emailError)) return emailError;
        return PasswordValidator.GetError(request.Password);
    }
    
    public static string GetErrorUpdateRequest(UserUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName)) return "FirstName or LastName cannot be empty.";

        string emailError = EmailValidator.GetError(request.Email);
        if (!string.IsNullOrEmpty(emailError)) return emailError;
        
        return string.Empty;
    }
}
  