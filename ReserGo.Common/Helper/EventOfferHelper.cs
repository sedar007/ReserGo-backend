using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class EventOfferHelper {
    public static EventOfferDto ToDto(this EventOffer occasion) {
        return new EventOfferDto {
            Id = occasion.Id,
            Description = occasion.Description,
            PricePerPerson = occasion.PricePerPerson,
            GuestLimit = occasion.GuestLimit,
            OfferStartDate = occasion.OfferStartDate,
            OfferEndDate = occasion.OfferEndDate,
            IsActive = occasion.IsActive,
            EventId = occasion.EventId,
            Event = occasion.Event.ToDto()
        };
    }
}