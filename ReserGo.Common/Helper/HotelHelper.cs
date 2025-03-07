using Common.DTO;
using Common.Entity;

namespace Common.Helper;

public static class HotelHelper {
    public static HotelDTO ToDto(this Hotel hotel) {
        return new HotelDTO {
            Id = hotel.Id,
            Name = hotel.Name,
            Location = hotel.Location,
            Capacity = hotel.Capacity
        };
    }
}