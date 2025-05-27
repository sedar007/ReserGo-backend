using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Occasion;

namespace ReserGo.Business.Validator;

public static class OccasionOfferValidator {
    public static string GetError(OccasionOfferCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerPerson <= 0) return "Price must be greater than zero.";
        if (request.GuestLimit <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.OccasionId == Guid.Empty) return "Occasion ID cannot be empty.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        return string.Empty;
    }

    public static string GetError(OccasionOfferUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerPerson <= 0) return "Price must be greater than zero.";
        if (request.GuestLimit <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        return string.Empty;
    }
}