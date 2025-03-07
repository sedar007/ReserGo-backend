using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class HotelHelper {
    public static HotelDto ToDto(this Hotel hotel) {
        return new HotelDto {
            Id = hotel.Id,
            Name = hotel.Name,
            Location = hotel.Location,
            Capacity = hotel.Capacity
        };
    }
}