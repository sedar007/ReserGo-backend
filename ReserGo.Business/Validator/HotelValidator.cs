using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.User;

namespace ReserGo.Business.Validator;

public static class HotelValidator {
    public static string GetErrorCreationRequest(HotelCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return "";
    }
    
    /* public static string GetErrorUpdateRequest(UserUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName)) return "FirstName or LastName cannot be empty.";

        string emailError = EmailValidator.GetError(request.Email);
        if (string.IsNullOrEmpty(emailError) == false) return emailError;
        
        return string.Empty;
    }*/ 
}
  