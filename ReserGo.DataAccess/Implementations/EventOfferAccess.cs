using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class EventOfferDataAccess : IEventOfferDataAccess {
    private readonly ReserGoContext _context;

    public EventOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<EventOffer?> GetById(Guid id) {
        return await _context.EventOffer
            .Include(h => h.Event)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<EventOffer>> GetEventsOfferByUserId(Guid userId) {
        return await _context.EventOffer
            .Where(x => x.UserId == userId)
            .Include(h => h.Event)
            .ToListAsync();
    }

    public async Task<EventOffer> Create(EventOffer eventOffer) {
        var newData = _context.EventOffer.Add(eventOffer);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullDataException("Error creating new @event offer.");
    }

    public async Task<EventOffer> Update(EventOffer eventOffer) {
        _context.EventOffer.Update(eventOffer);
        await _context.SaveChangesAsync();
        return eventOffer;
    }

    public async Task Delete(EventOffer eventOffer) {
        _context.EventOffer.Remove(eventOffer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<EventOffer>> SearchAvailability(EventSearchAvailabilityRequest request) {
        return await _context.EventOffer
            .Include(o => o.Event)
            .Include(o => o.Bookings)
            .OrderByDescending(o => o.OfferStartDate)
            .Where(o => o.OfferStartDate <= request.StartDate &&
                        o.OfferEndDate >= request.EndDate &&
                        o.GuestLimit >= request.NumberOfGuests &&
                        !o.Bookings.Any(b => b.StartDate <= request.EndDate &&
                                             b.EndDate >= request.StartDate))
            .ToListAsync();
    }
}