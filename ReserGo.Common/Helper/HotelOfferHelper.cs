using ReserGo.Common.DTO;
using ReserGo.Common.Entity;

namespace ReserGo.Common.Helper;

public static class HotelOfferHelper {
    public static HotelOfferDto ToDto(this HotelOffer hotel) {
        return new HotelOfferDto {
            Id = hotel.Id,
            OfferTitle = hotel.OfferTitle,
            Description = hotel.Description,
            PricePerNight = hotel.PricePerNight,
            NumberOfGuests = hotel.NumberOfGuests,
            NumberOfRooms = hotel.NumberOfRooms,
            OfferStartDate = hotel.OfferStartDate,
            OfferEndDate = hotel.OfferEndDate,
            IsActive = hotel.IsActive,
            HotelId = hotel.HotelId,
            Hotel = hotel.Hotel.ToDto()
        };
    }
}