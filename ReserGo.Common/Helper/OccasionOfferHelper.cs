using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class OccasionOfferHelper {
    public static OccasionOfferDto ToDto(this OccasionOffer occasion) {
        return new OccasionOfferDto {
            Id = occasion.Id,
            OfferTitle = occasion.OfferTitle,
            Description = occasion.Description,
            Price = occasion.Price,
            NumberOfGuests = occasion.NumberOfGuests,
            OfferStartDate = occasion.OfferStartDate,
            OfferEndDate = occasion.OfferEndDate,
            IsActive = occasion.IsActive,
            OccasionId = occasion.OccasionId,
            Occasion = occasion.Occasion.ToDto()
        };
    }
}