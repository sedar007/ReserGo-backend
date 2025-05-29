using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Event;

namespace ReserGo.Business.Interfaces;

public interface IEventService {
    Task<EventDto?> GetById(Guid id);
    Task<EventDto?> GetByStayId(long stayId);
    Task<IEnumerable<EventDto>> GetEventsByUserId(Guid userId);
    Task<EventDto> Create(EventCreationRequest request);
    Task<EventDto> Update(long stayId, EventUpdateRequest request);
    Task Delete(Guid id);
}