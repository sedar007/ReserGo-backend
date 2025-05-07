using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.Business.Validator;

public static class RestaurantValidator {
    public static string GetError(RestaurantCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return StayIdValidator.Check(request.StayId, 2);
    }

    public static string GetError(RestaurantUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return "";
    }
}