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
}