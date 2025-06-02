using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;

namespace ReserGo.Business.Interfaces;

public interface IEventOfferService {
    Task<EventOfferDto?> GetById(Guid id);
    Task<EventOfferDto> Create(EventOfferCreationRequest request);
    Task<EventOfferDto> Update(Guid id, EventOfferUpdateRequest request);
    Task<IEnumerable<EventOfferDto>> GetEventsByUserId(Guid userId);
    Task<IEnumerable<EventAvailabilityResponse>> SearchAvailability(EventSearchAvailabilityRequest request);

    Task Delete(Guid id);
}