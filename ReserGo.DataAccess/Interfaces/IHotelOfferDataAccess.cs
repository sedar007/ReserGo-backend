using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IHotelOfferDataAccess {
    Task<HotelOffer?> GetById(int id);
    Task<HotelOffer> Create(HotelOffer hotelOffer);
    Task<HotelOffer> Update(HotelOffer hotelOffer);
    Task<IEnumerable<HotelOffer>> GetHotelsOfferByUserId(int userId);
    Task Delete(HotelOffer hotelOffer);
}