using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.Products.Occasion;

namespace ReserGo.Business.Validator;

public static class OccasionValidator {
    public static string GetError(OccasionCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return StayIdValidator.Check(request.StayId, 3);
    }

    public static string GetError(OccasionUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return "";
    }
}