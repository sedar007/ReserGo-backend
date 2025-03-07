using Common.DTO;
using Common.Entity;

namespace Common.Helper;
public static class RestaurantHelper {
    public static RestaurantDTO ToDto(this Restaurant restaurant) {
        return new RestaurantDTO {
            Id = restaurant.Id,
            Name = restaurant.Name,
            CuisineType = restaurant.CuisineType,
            Capacity = restaurant.Capacity
        };
    }
}