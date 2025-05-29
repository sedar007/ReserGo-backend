using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.Products.Event;

namespace ReserGo.Business.Validator;

public static class EventValidator {
    public static string GetError(EventCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return StayIdValidator.Check(request.StayId, 3);
    }

    public static string GetError(EventUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Location)) return "Location cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        return "";
    }
}