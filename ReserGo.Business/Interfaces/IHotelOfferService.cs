using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.Business.Interfaces;

public interface IHotelOfferService {
    Task<HotelOfferDto?> GetById(int id);
    Task<HotelOfferDto> Create(HotelOfferCreationRequest request);
    Task<HotelOfferDto> Update(int id, HotelOfferUpdateRequest request);
    Task<IEnumerable<HotelOfferDto>> GetHotelsByUserId(int userId);
    Task Delete(int id);
}