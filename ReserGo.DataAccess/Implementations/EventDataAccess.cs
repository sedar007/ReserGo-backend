using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class EventDataAccess : IEventDataAccess {
    private readonly ReserGoContext _context;

    public EventDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Event?> GetById(Guid id) {
        return await _context.Event.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Event?> GetByStayId(long stayId) {
        return await _context.Event.FirstOrDefaultAsync(x => x.StayId == stayId);
    }

    public async Task<IEnumerable<Event>> GetEventsByUserId(Guid userId) {
        return await _context.Event.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Event> Create(Event user) {
        var newData = _context.Event.Add(user);
        await _context.SaveChangesAsync();
        return await GetByStayId(newData.Entity.StayId) ??
               throw new NullReferenceException("Error creating new @event.");
    }

    public async Task<Event> Update(Event @event) {
        _context.Event.Update(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task Delete(Event @event) {
        _context.Event.Remove(@event);
        await _context.SaveChangesAsync();
    }
}