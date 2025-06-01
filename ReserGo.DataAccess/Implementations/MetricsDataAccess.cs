using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.Common.Enum;
using ReserGo.Common.Response;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class MetricsDataAccess : IMetricsDataAccess {
    private readonly ReserGoContext _context;

    public MetricsDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Room?> GetById(Guid id) {
        return await _context.Room.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Room>> GetRoomsByHotelId(Guid hotelId) {
        return await _context.Room.Where(x => x.HotelId == hotelId).ToListAsync();
    }

    //Task<MetricsResponse> GetMetricsMonths(Product product, Guid userId);

    public async Task<MetricsResponse> GetMetricsMonths(Product product, Guid userId) {
        throw new NotImplementedException();
        //return await _context.BookingHotel.Where(x => x.HotelId == hotelId).ToListAsync();
    }

    public async Task<Room> Create(Room room) {
        var newData = _context.Room.Add(room);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullReferenceException("Error creating new room.");
    }

    public async Task<Room> Update(Room room) {
        _context.Room.Update(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task Delete(Room room) {
        _context.Room.Remove(room);
        await _context.SaveChangesAsync();
    }
}