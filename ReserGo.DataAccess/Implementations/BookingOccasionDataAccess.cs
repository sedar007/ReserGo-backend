using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class BookingOccasionDataAccess : IBookingOccasionDataAccess {
    private readonly ReserGoContext _context;

    public BookingOccasionDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<BookingOccasion> Create(BookingOccasion bookingOccasion) {
        await _context.BookingOccasion.AddAsync(bookingOccasion);
        await _context.SaveChangesAsync();
        return bookingOccasion;
    }

    public async Task<BookingOccasion?> GetById(Guid id) {
        return await _context.BookingOccasion
            //.Include(x => x.OccasionOffer)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    
    public async Task<IEnumerable<BookingOccasion>> GetBookingsByUserId(Guid userId) {
        return await _context.BookingOccasion
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<BookingOccasion>> GetBookingsByAdminId(Guid adminId) {
        return await _context.BookingOccasion
            .Include(b => b.Occasion)
            .Where(b => b.Occasion.UserId == adminId)
            .ToListAsync();
    }
    
    public async Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId,
        DateTime startDate, DateTime endDate) {
        return await _context.BookingOccasion
            .Include(b => b.Occasion)
            .Where(b => b.Occasion.UserId == adminId)
            .Where(b => startDate.Date >= b.BookingDate && endDate.Date <= b.BookingDate)
            .CountAsync();
    }
    
    public async Task<int> GetNbBookingsLast30Days(Guid adminId) {
        var today = DateTime.UtcNow;
        var days30Before = today.AddDays(-30);

        return await _context.BookingOccasion
            .Include(b => b.Occasion)
            .Where(b => b.Occasion.UserId == adminId)
            .Where(b => b.BookingDate >= days30Before || b.BookingDate > today)
            .CountAsync();
    }
    


}