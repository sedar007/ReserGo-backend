using ReserGo.Common.Requests.Products.Hotel.Rooms;

namespace ReserGo.Business.Validator;

public static class RoomValidator {
    public static string GetError(RoomCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.RoomNumber)) return "Room number cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.HotelId.ToString())) return "Hotel ID cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        if (request.PricePerNight <= 0) return "Price per night must be greater than zero.";
        if (string.IsNullOrWhiteSpace(request.IsAvailable.ToString())) return "Availability cannot be null.";
        return "";
    }

    public static string GetError(RoomUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.RoomNumber)) return "Room number cannot be empty.";
        if (request.Capacity == 0) return "Capacity cannot be null or zero.";
        if (request.PricePerNight <= 0) return "Price per night must be greater than zero.";
        if (string.IsNullOrWhiteSpace(request.IsAvailable.ToString())) return "Availability cannot be null.";
        return "";
    }
}