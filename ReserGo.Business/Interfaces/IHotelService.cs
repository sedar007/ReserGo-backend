using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products;
using ReserGo.Common.Requests.Products.Hotel;

namespace ReserGo.Business.Interfaces;

public interface IHotelService {
    Task<HotelDto?> GetById(Guid id);
    Task<HotelDto?> GetByStayId(long stayId);
    Task<HotelDto> Create(HotelCreationRequest request);
    Task<HotelDto> Update(long stayId, HotelUpdateRequest request);
    Task<IEnumerable<HotelDto>> GetHotelsByUserId(Guid userId);
    Task Delete(Guid id);
}