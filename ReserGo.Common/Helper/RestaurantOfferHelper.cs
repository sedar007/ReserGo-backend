using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class RestaurantOfferHelper {
    public static RestaurantOfferDto ToDto(this RestaurantOffer restaurant) {
        return new RestaurantOfferDto {
            Id = restaurant.Id,
            Description = restaurant.Description,
            PricePerPerson = restaurant.PricePerPerson,
            GuestLimit = restaurant.GuestLimit,
            OfferStartDate = restaurant.OfferStartDate,
            OfferEndDate = restaurant.OfferEndDate,
            IsActive = restaurant.IsActive,
            RestaurantId = restaurant.RestaurantId,
            Restaurant = restaurant.Restaurant.ToDto(),
            UserId = restaurant.UserId
        };
    }
}