using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class HotelDataAccess : IHotelDataAccess {
    
    private readonly ReserGoContext _context;
    
    public HotelDataAccess(ReserGoContext context) {
        _context = context;
    }
    
    public async Task<Hotel?> GetById(int id) {
        return await _context.Hotel.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Hotel> Create(Hotel user) {
        EntityEntry<Hotel> newData = _context.Hotel.Add(user);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ?? throw new NullReferenceException("Error creating new hotel.");
    }
}

