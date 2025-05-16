using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class RoomHelper {
    public static RoomDto ToDto(this Room room) {
        return new RoomDto {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Capacity = room.Capacity,
            PricePerNight = room.PricePerNight,
            IsAvailable = room.IsAvailable
        };
    }
}