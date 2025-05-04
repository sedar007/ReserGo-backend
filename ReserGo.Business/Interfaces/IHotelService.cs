using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products;

namespace ReserGo.Business.Interfaces;

public interface IHotelService {
    Task<HotelDto?> GetById(int id);
    Task<HotelDto?> GetByStayId(long stayId);
    Task<HotelDto> Create(HotelCreationRequest request);
    Task<HotelDto> Update(long stayId, HotelUpdateRequest request);
    Task Delete(int id);
}