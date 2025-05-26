using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class OccasionOfferHelper {
    public static OccasionOfferDto ToDto(this OccasionOffer occasion) {
        return new OccasionOfferDto {
            Id = occasion.Id,
            Description = occasion.Description,
            PricePerPerson = occasion.PricePerPerson,
            GuestLimit = occasion.GuestLimit,
            OfferStartDate = occasion.OfferStartDate,
            OfferEndDate = occasion.OfferEndDate,
            IsActive = occasion.IsActive,
            OccasionId = occasion.OccasionId,
            Occasion = occasion.Occasion.ToDto()
        };
    }
}