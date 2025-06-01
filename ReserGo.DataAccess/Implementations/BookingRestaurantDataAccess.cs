using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class BookingRestaurantDataAccess : IBookingRestaurantDataAccess {
    private readonly ReserGoContext _context;

    public BookingRestaurantDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant) {
        await _context.BookingRestaurant.AddAsync(bookingRestaurant);
        await _context.SaveChangesAsync();
        return bookingRestaurant;
    }

    public async Task<BookingRestaurant?> GetById(Guid id) {
        return await _context.BookingRestaurant
            .Include(x => x.RestaurantOffer)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId,
        DateOnly startDate, DateOnly endDate) {
        return await _context.BookingRestaurant
            .Include(b => b.Restaurant)
            .Where(b => b.Restaurant.UserId == adminId)
            .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
            .CountAsync();
    }
    
   /* public async Task<int> GetNbBookingsLast30Days(Guid adminId) {
        var today = DateTime.UtcNow;
        var days30Before = today.AddDays(-30);

        return await _context.BookingRestaurant
            .Include(b => b.RestaurantOffer)
            .Where(b => b.RestaurantOffer.UserId == adminId)
            .Where(b => b.BookingDate >= days30Before || b.BookingDate > today)
            .CountAsync();
    } */

    public async Task<IEnumerable<BookingRestaurant>> GetBookingsByUserId(Guid userId) {
        return await _context.BookingRestaurant
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingRestaurant>> GetBookingsByAdminId(Guid adminId) {
        return await _context.BookingRestaurant
            .Include(b => b.Restaurant)
            .Where(b => b.Restaurant.UserId == adminId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<BookingRestaurant>> GetBookingYearsByUserId(Guid userId) {
        var currentYear = DateTime.UtcNow.Year;
        return await _context.BookingRestaurant
            .Include(b => b.Restaurant)
            .Where(b => b.Restaurant.UserId == userId && b.BookingDate.Year == currentYear)
            .ToListAsync();
    }
}