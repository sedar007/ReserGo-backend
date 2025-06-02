using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class EventOfferHelper {
    public static EventOfferDto ToDto(this EventOffer occasion) {
        return new EventOfferDto {
            Id = occasion.Id,
            Description = occasion.Description,
            PricePerDay = occasion.PricePerDay,
            GuestLimit = occasion.GuestLimit,
            OfferStartDate = occasion.OfferStartDate,
            OfferEndDate = occasion.OfferEndDate,
            IsActive = occasion.IsActive,
            EventId = occasion.EventId,
            UserId = occasion.UserId,
            Event = occasion.Event.ToDto()
        };
    }
}