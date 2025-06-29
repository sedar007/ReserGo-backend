using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class RoomAvailabilityHelper {
    public static RoomAvailabilityDto ToDto(this RoomAvailability roomAvailability) {
        return new RoomAvailabilityDto {
            Id = roomAvailability.Id,
            Description = roomAvailability.Description,
            Hotel = roomAvailability.Hotel.ToDto(),
            Room = roomAvailability.Room.ToDto(),
            StartDate = roomAvailability.StartDate,
            EndDate = roomAvailability.EndDate
        };
    }
}