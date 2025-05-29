using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class EventOfferDataAccess : IEventOfferDataAccess {
    private readonly ReserGoContext _context;

    public EventOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<EventOffer?> GetById(Guid id) {
        return await _context.EventOffer.Include(h => h.Event).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<EventOffer>> GetEventsOfferByUserId(Guid userId) {
        return await _context.EventOffer.Where(x => x.UserId == userId).Include(h => h.Event).ToListAsync();
    }

    public async Task<EventOffer> Create(EventOffer user) {
        EntityEntry<EventOffer> newData = _context.EventOffer.Add(user);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullReferenceException("Error creating new @event offer.");
    }

    public async Task<EventOffer> Update(EventOffer eventOffer) {
        _context.EventOffer.Update(eventOffer);
        await _context.SaveChangesAsync();
        return eventOffer;
    }

    public async Task Delete(EventOffer @event) {
        _context.EventOffer.Remove(@event);
        await _context.SaveChangesAsync();
    }
}