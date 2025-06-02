using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class BookingRestaurantDataAccess : IBookingRestaurantDataAccess {
    private readonly ReserGoContext _context;

    public BookingRestaurantDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<BookingRestaurant> Create(BookingRestaurant bookingRestaurant) {
        var data = await _context.BookingRestaurant.AddAsync(bookingRestaurant);
        await _context.SaveChangesAsync();
        return await GetById(data.Entity.Id) ??
               throw new NullDataException("Error creating new booking restaurant.");
    }

    public async Task<BookingRestaurant?> GetById(Guid id) {
        return await _context.BookingRestaurant
            .Include(x => x.Restaurant)
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

    public async Task<IEnumerable<BookingRestaurant>> GetBookingsByUserId(Guid userId, int pageSize) {
        return await _context.BookingRestaurant
            .Include(b => b.Restaurant)
            .OrderByDescending(b => b.Date)
            .Where(b => b.UserId == userId)
            .Take(pageSize)
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