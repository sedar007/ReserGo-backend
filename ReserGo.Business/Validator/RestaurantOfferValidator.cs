using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Restaurant;

namespace ReserGo.Business.Validator;

public static class RestaurantOfferValidator {
    public static string GetError(RestaurantOfferCreationRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.OfferTitle)) return "Offer title cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerPerson <= 0) return "Price per person must be greater than zero.";
        if (request.NumberOfGuests <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        if (request.IsActive != null && request.IsActive == false && request.OfferEndDate < DateTime.Now)
            return "Cannot deactivate an expired offer.";
        if (request.OfferEndDate < DateTime.Now) return "Offer end date cannot be in the past.";
        if (request.OfferStartDate < DateTime.Now) return "Offer start date cannot be in the past.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.RestaurantId == Guid.Empty) return "Restaurant ID cannot be empty.";
        return "";
    }

    public static string GetError(RestaurantOfferUpdateRequest? request) {
        if (request == null) return "Invalid request.";
        if (string.IsNullOrWhiteSpace(request.OfferTitle)) return "Offer title cannot be empty.";
        if (string.IsNullOrWhiteSpace(request.Description)) return "Description cannot be empty.";
        if (request.PricePerPerson <= 0) return "Price per person must be greater than zero.";
        if (request.NumberOfGuests <= 0) return "Number of guests must be greater than zero.";
        if (request.OfferStartDate == default) return "Offer start date is invalid.";
        if (request.OfferEndDate == default) return "Offer end date is invalid.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        if (request.IsActive == null) return "IsActive cannot be null.";
        if (request.IsActive != null && request.IsActive == false && request.OfferEndDate < DateTime.Now)
            return "Cannot deactivate an expired offer.";
        if (request.OfferEndDate < DateTime.Now) return "Offer end date cannot be in the past.";
        if (request.OfferStartDate < DateTime.Now) return "Offer start date cannot be in the past.";
        if (request.OfferStartDate >= request.OfferEndDate) return "Offer start date must be before end date.";
        return "";
    }
}