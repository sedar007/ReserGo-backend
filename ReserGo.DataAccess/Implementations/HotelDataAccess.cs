using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class HotelDataAccess : IHotelDataAccess {
    private readonly ReserGoContext _context;

    public HotelDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Hotel?> GetById(Guid id) {
        return await _context.Hotel.Include(x => x.Rooms).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Hotel?> GetByStayId(long stayId) {
        return await _context.Hotel.Include(x => x.Rooms).FirstOrDefaultAsync(x => x.StayId == stayId);
    }

    public async Task<IEnumerable<Hotel>> GetHotelsByUserId(Guid userId) {
        return await _context.Hotel.Include(x => x.Rooms).Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Hotel> Create(Hotel user) {
        var newData = _context.Hotel.Add(user);
        await _context.SaveChangesAsync();
        return await GetByStayId(newData.Entity.StayId) ??
               throw new NullReferenceException("Error creating new hotel.");
    }

    public async Task<Hotel> Update(Hotel hotel) {
        _context.Hotel.Update(hotel);
        await _context.SaveChangesAsync();
        return hotel;
    }

    public async Task<bool> IsAuthorized(Guid hotelId, Guid userId) {
        return await _context.Hotel.AnyAsync(x => x.Id == hotelId && x.UserId == userId);
    }

    public async Task Delete(Hotel hotel) {
        _context.Hotel.Remove(hotel);
        await _context.SaveChangesAsync();
    }
}