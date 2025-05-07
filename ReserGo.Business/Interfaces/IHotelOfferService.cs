using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.Business.Interfaces;

public interface IHotelOfferService {
    Task<HotelOfferDto?> GetById(Guid id);
    Task<HotelOfferDto> Create(HotelOfferCreationRequest request);
    Task<HotelOfferDto> Update(Guid id, HotelOfferUpdateRequest request);
    Task<IEnumerable<HotelOfferDto>> GetHotelsByUserId(Guid userId);
    Task Delete(Guid id);
}