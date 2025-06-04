using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class EventHelper {
    public static EventDto ToDto(this Event occasion) {
        return new EventDto {
            Id = occasion.Id,
            Name = occasion.Name,
            Capacity = occasion.Capacity,
            Location = occasion.Location,
            StayId = occasion.StayId,
            Picture = occasion.Picture ?? "system/a3bfhmv9txtzdbyxsqki",
            LastUpdated = occasion.LastUpdated
        };
    }
}