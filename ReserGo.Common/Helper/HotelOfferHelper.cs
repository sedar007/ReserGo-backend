using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class HotelOfferHelper {
    public static HotelOfferDto ToDto(this HotelOffer hotelOffer) {
        return new HotelOfferDto {
            Id = hotelOffer.Id,
            OfferTitle = hotelOffer.OfferTitle,
            Description = hotelOffer.Description,
            PricePerNight = hotelOffer.PricePerNight,
            NumberOfGuests = hotelOffer.NumberOfGuests,
            NumberOfRooms = hotelOffer.NumberOfRooms,
            OfferStartDate = hotelOffer.OfferStartDate,
            OfferEndDate = hotelOffer.OfferEndDate,
            IsActive = hotelOffer.IsActive,
            HotelId = hotelOffer.HotelId,
            UserId = hotelOffer.UserId,
            Hotel = hotelOffer.Hotel?.ToDto()
        };
    }
}