using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.Business.Validator;

public static class HotelOfferValidator {
    public static string GetError(HotelOfferCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.OfferTitle)) return "Offer title cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerNight <= 0) return "Price per night must be greater than zero.";
        if (request.NumberOfGuests <= 0) return "Number of guests must be greater than zero.";
        if (request.NumberOfRooms <= 0) return "Number of rooms must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.HotelId == Guid.Empty) return "Hotel ID must be a valid GUID.";
        return "";
    }

    public static string GetError(HotelOfferUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.OfferTitle)) return "Offer title cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerNight <= 0) return "Price per night must be greater than zero.";
        if (request.NumberOfGuests <= 0) return "Number of guests must be greater than zero.";
        if (request.NumberOfRooms <= 0) return "Number of rooms must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (!request.IsActive &&
            request.OfferEndDate < DateOnly.FromDateTime(DateTime.Now))
            return "Cannot deactivate an expired offer.";
        if (request.OfferEndDate < DateOnly.FromDateTime(DateTime.Now)) return "Offer end date cannot be in the past.";
        if (request.OfferStartDate < DateOnly.FromDateTime(DateTime.Now))
            return "Offer start date cannot be in the past.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        return "";
    }
}