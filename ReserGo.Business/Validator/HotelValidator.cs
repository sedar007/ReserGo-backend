using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.Business.Validator;

public static class HotelValidator {
    public static string GetError(HotelCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        return StayIdValidator.Check(request.StayId, 1);
    }

    public static string GetError(HotelUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        return "";
    }
}