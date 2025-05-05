using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;
public static class RestaurantHelper {
    public static RestaurantDto ToDto(this Restaurant restaurant) {
        return new RestaurantDto {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Location = restaurant.Location,
            CuisineType = restaurant.CuisineType,
            Capacity = restaurant.Capacity,
            StayId = restaurant.StayId,
            Picture = restaurant.Picture,
            LastUpdated = restaurant.LastUpdated
        };
    }
}