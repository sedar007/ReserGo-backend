using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class RoomAvailabilityDataAccess : IRoomAvailabilityDataAccess {
    private readonly ReserGoContext _context;

    public RoomAvailabilityDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<RoomAvailability?> GetById(Guid id) {
        return await _context.RoomAvailability.Include(x => x.Room).Include(x => x.Hotel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<RoomAvailability> Create(RoomAvailability roomAvailability) {
        var newData = _context.RoomAvailability.Add(roomAvailability);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullReferenceException("Error creating new RoomAvailability");
    }

    public async Task<RoomAvailability?> GetByRoomId(Guid roomId) {
        return await _context.RoomAvailability.Include(x => x.Room).Include(x => x.Hotel)
            .FirstOrDefaultAsync(x => x.RoomId == roomId);
    }

    public async Task<RoomAvailability> Update(RoomAvailability roomAvailability) {
        var updatedData = _context.RoomAvailability.Update(roomAvailability);
        await _context.SaveChangesAsync();
        return await GetById(updatedData.Entity.Id) ??
               throw new NullReferenceException("Error updating RoomAvailability");
    }

    public async Task Delete(RoomAvailability roomAvailability) {
        _context.RoomAvailability.Remove(roomAvailability);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoomAvailability>> GetAvailabilitiesByHotelId(Guid hotelId, int skip, int take) {
        return await _context.RoomAvailability
            .Include(ra => ra.Room)
            .Include(ra => ra.Hotel)
            .Where(ra => ra.HotelId == hotelId)
            .OrderByDescending(ra => ra.StartDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoomAvailability>> GetAvailabilitiesByHotelIds(IEnumerable<Guid> hotelIds, int skip,
        int take) {
        return await _context.RoomAvailability
            .Include(ra => ra.Room)
            .Include(ra => ra.Hotel)
            .Where(ra => hotelIds.Contains(ra.HotelId))
            .OrderBy(ra => ra.StartDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}