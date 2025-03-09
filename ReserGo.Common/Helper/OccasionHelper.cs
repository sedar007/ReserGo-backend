using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;
public static class OccasionHelper {
    public static OccasionDto ToDto(this Occasion occasion) {
        return new OccasionDto {
            Id = occasion.Id,
            Name = occasion.Name,
            StartDate = occasion.StartDate,
            EndDate = occasion.EndDate,
            Capacity = occasion.Capacity,
            Location = occasion.Location
        };
    }
}