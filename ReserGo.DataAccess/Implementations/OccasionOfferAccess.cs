using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class OccasionOfferDataAccess : IOccasionOfferDataAccess {
    
    private readonly ReserGoContext _context;
    
    public OccasionOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<OccasionOffer?> GetById(int id) {
        return await _context.OccasionOffer.
            Include(h => h.Occasion).
            FirstOrDefaultAsync(x => x.Id ==  id);
    }
    
    public async Task<IEnumerable<OccasionOffer>> GetOccasionsOfferByUserId(int userId) {
        return await _context.OccasionOffer.Where(x => x.UserId ==  userId).
            Include(h => h.Occasion).
            ToListAsync();
    }
    
    public async Task<OccasionOffer> Create(OccasionOffer user) {
        EntityEntry<OccasionOffer> newData = _context.OccasionOffer.Add(user);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ?? throw new NullReferenceException("Error creating new occasion offer.");
    }
    
    public async Task<OccasionOffer> Update(OccasionOffer occasionOffer) {
        _context.OccasionOffer.Update(occasionOffer);
        await _context.SaveChangesAsync();
        return occasionOffer;
    }
    
    public async Task Delete(OccasionOffer occasion) {
        _context.OccasionOffer.Remove(occasion);
        await _context.SaveChangesAsync();
    }
}

