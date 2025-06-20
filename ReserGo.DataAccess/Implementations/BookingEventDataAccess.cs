using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class BookingEventDataAccess : IBookingEventDataAccess {
    private readonly ReserGoContext _context;

    public BookingEventDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<BookingEvent> Create(BookingEvent bookingEvent) {
        var newData = await _context.BookingEvent.AddAsync(bookingEvent);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullDataException("Error creating new booking event.");
    }

    public async Task<BookingEvent?> GetById(Guid id) {
        return await _context.BookingEvent
            .Include(x => x.Event)
            .Include(x => x.EventOffer)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<IEnumerable<BookingEvent>> GetBookingsByUserId(Guid userId, int pageSize = Consts.DefaultPageSize) {
        return await _context.BookingEvent
            .Include(b => b.Event)
            .OrderByDescending(b => b.BookingDate)
            .Where(b => b.UserId == userId)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingEvent>> GetBookingsByAdminId(Guid adminId) {
        return await _context.BookingEvent
            .Include(b => b.Event)
            .Include(b => b.User)
            .Where(b => b.Event.UserId == adminId)
            .OrderByDescending(b=> b.BookingDate)
            .ToListAsync();
    }

    public async Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId,
        DateOnly startDate, DateOnly endDate) {
        var res = await _context.BookingEvent
            .Include(b => b.Event)
            .Where(b => b.Event.UserId == adminId)
            .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
            .CountAsync();
        return res;
    }

    public async Task<IEnumerable<BookingEvent>> GetBookingYearsByUserId(Guid userId) {
        var currentYear = DateTime.UtcNow.Year;
        return await _context.BookingEvent
            .Include(b => b.Event)
            .Where(b => b.Event.UserId == userId && b.BookingDate.Year == currentYear)
            .ToListAsync();
    }
}