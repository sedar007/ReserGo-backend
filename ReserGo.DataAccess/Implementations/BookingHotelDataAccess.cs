using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class BookingHotelDataAccess : IBookingHotelDataAccess {
    private readonly ReserGoContext _context;

    public BookingHotelDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<BookingHotel> Create(BookingHotel bookingHotel) {
        await _context.BookingHotel.AddAsync(bookingHotel);
        await _context.SaveChangesAsync();
        return bookingHotel;
    }

    public async Task<BookingHotel?> GetById(Guid id) {
        return await _context.BookingHotel
            //.Include(x => x.HotelOffer)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<BookingHotel>> GetBookingsByRoomId(Guid roomId) {
        return await _context.BookingHotel
            .Where(b => b.RoomId == roomId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingHotel>> GetBookingsByUserId(Guid userId) {
        return await _context.BookingHotel
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingHotel>> GetBookingsByAdminId(Guid adminId) {
        return await _context.BookingHotel
            .Include(b => b.Hotel)
            .Include(r => r.Room)
            .Where(b => b.Hotel.UserId == adminId)
            .ToListAsync();
    }

    public async Task<int> GetNbBookingBetween2DatesByAdminId(Guid adminId,
        DateTime startDate, DateTime endDate) {
        return await _context.BookingHotel
            .Include(b => b.Hotel)
            .Where(b => b.Hotel.UserId == adminId)
            .Where(b => b.BookingDate >= startDate.Date && b.BookingDate <= endDate.Date)
            .CountAsync();
    }

    public async Task<int> GetNbBookingsLast30Days(Guid adminId) {
        var today = DateTime.UtcNow;
        var days30Before = today.AddDays(-30);

        return await _context.BookingHotel
            .Include(b => b.Hotel)
            .Where(b => b.Hotel.UserId == adminId)
            .Where(b => b.BookingDate >= days30Before || b.BookingDate > today)
            .CountAsync();
    }
    
    public async Task<IEnumerable<BookingHotel>> GetBookingYearsByUserId(Guid userId) {
        var currentYear = DateTime.UtcNow.Year;
        return await _context.BookingHotel
            .Include(b => b.Hotel)
            .Include(b=> b.BookingDate)
            .Where(b => b.Hotel.UserId == userId && b.BookingDate.Year == currentYear)
            .ToListAsync();
    }
}