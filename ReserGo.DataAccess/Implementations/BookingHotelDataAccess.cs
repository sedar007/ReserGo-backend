using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class BookingHotelDataAccess  : IBookingHotelDataAccess {
    
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
            .Include(x => x.HotelOffer)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
}