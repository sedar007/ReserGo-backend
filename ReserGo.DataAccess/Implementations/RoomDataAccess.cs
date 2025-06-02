using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class RoomDataAccess : IRoomDataAccess {
    private readonly ReserGoContext _context;

    public RoomDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Room?> GetById(Guid id) {
        return await _context.Room.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Room>> GetRoomsByHotelId(Guid hotelId) {
        return await _context.Room.Where(x => x.HotelId == hotelId).ToListAsync();
    }

    public async Task<Room> Create(Room room) {
        var newData = _context.Room.Add(room);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullDataException("Error creating new room.");
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