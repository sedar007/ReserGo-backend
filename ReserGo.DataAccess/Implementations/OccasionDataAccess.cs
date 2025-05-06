using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class OccasionDataAccess : IOccasionDataAccess {
    private readonly ReserGoContext _context;

    public OccasionDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Occasion?> GetById(int id) {
        return await _context.Occasion.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Occasion?> GetByStayId(long stayId) {
        return await _context.Occasion.FirstOrDefaultAsync(x => x.StayId == stayId);
    }

    public async Task<IEnumerable<Occasion>> GetOccasionsByUserId(int userId) {
        return await _context.Occasion.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Occasion> Create(Occasion user) {
        EntityEntry<Occasion> newData = _context.Occasion.Add(user);
        await _context.SaveChangesAsync();
        return await GetByStayId(newData.Entity.StayId) ??
               throw new NullReferenceException("Error creating new occasion.");
    }

    public async Task<Occasion> Update(Occasion occasion) {
        _context.Occasion.Update(occasion);
        await _context.SaveChangesAsync();
        return occasion;
    }

    public async Task Delete(Occasion occasion) {
        _context.Occasion.Remove(occasion);
        await _context.SaveChangesAsync();
    }
}