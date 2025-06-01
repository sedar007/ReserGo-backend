using ReserGo.Common.Requests.Products.Event;

namespace ReserGo.Business.Validator;

public static class EventOfferValidator {
    public static string GetError(EventOfferCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerDay <= 0) return "Price must be greater than zero.";
        if (request.GuestLimit <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.EventId == Guid.Empty) return "Event ID cannot be empty.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        return string.Empty;
    }

    public static string GetError(EventOfferUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerDay <= 0) return "Price must be greater than zero.";
        if (request.GuestLimit <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        return string.Empty;
    }
}