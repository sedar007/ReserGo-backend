using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IEventDataAccess {
    Task<Event?> GetById(Guid id);
    Task<Event?> GetByStayId(long stayId);
    Task<IEnumerable<Event>> GetEventsByUserId(Guid userId);
    Task<Event> Create(Event eventData);
    Task<Event> Update(Event eventData);
    Task Delete(Event @event);
}