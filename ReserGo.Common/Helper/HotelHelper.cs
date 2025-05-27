using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class HotelHelper {
    public static HotelDto ToDto(this Hotel hotel) {
        return new HotelDto {
            Id = hotel.Id,
            UserId = hotel.UserId,
            Name = hotel.Name,
            StayId = hotel.StayId,
            Location = hotel.Location,
            NumberOfRooms = hotel.NumberOfRooms,
            Description = hotel.Description,
            Picture = hotel.Picture,
            LastUpdated = hotel.LastUpdated,
            Rooms = hotel.Rooms?.Select(room => room.ToDto())
        };
    }
}