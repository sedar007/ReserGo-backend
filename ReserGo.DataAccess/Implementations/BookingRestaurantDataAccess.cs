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
}