using Common.DTO;
using Common.Entity;

namespace Common.Helper;
public static class OccasionHelper {
    public static OccasionDTO ToDto(this Occasion occasion) {
        return new OccasionDTO {
            Id = occasion.Id,
            Name = occasion.Name,
            StartDate = occasion.StartDate,
            EndDate = occasion.EndDate,
            Capacity = occasion.Capacity,
            Location = occasion.Location
        };
    }
}