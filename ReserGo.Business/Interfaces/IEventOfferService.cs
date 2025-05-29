using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Event;

namespace ReserGo.Business.Interfaces;

public interface IEventOfferService {
    Task<EventOfferDto?> GetById(Guid id);
    Task<EventOfferDto> Create(EventOfferCreationRequest request);
    Task<EventOfferDto> Update(Guid id, EventOfferUpdateRequest request);
    Task<IEnumerable<EventOfferDto>> GetEventsByUserId(Guid userId);
    Task Delete(Guid id);
}