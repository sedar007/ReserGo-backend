using Microsoft.AspNetCore.Http;

using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Requests.User;

namespace ReserGo.Business.Interfaces;

public interface IHotelService {
    Task<HotelDto?> GetById(int id);
    Task<HotelDto> Create(HotelCreationRequest request);
}