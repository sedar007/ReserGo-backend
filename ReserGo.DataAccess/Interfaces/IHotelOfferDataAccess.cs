using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IHotelOfferDataAccess {
    Task<HotelOffer?> GetById(Guid id);
    Task<HotelOffer> Create(HotelOffer hotelOffer);
    Task<HotelOffer> Update(HotelOffer hotelOffer);
    Task<IEnumerable<HotelOffer>> GetHotelsOfferByUserId(Guid userId);
    Task Delete(HotelOffer hotelOffer);
}