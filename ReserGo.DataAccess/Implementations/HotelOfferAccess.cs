using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class HotelOfferDataAccess : IHotelOfferDataAccess {
    
    private readonly ReserGoContext _context;
    
    public HotelOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<HotelOffer?> GetById(int id) {
        return await _context.HotelOffer.FirstOrDefaultAsync(x => x.Id ==  id);
    }
    
    public async Task<IEnumerable<HotelOffer>> GetHotelsOfferByUserId(int userId) {
        return await _context.HotelOffer.Where(x => x.UserId ==  userId).ToListAsync();
    }
    
    public async Task<HotelOffer> Create(HotelOffer user) {
        EntityEntry<HotelOffer> newData = _context.HotelOffer.Add(user);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ?? throw new NullReferenceException("Error creating new hotel offer.");
    }
    
    public async Task<HotelOffer> Update(HotelOffer hotelOffer) {
        _context.HotelOffer.Update(hotelOffer);
        await _context.SaveChangesAsync();
        return hotelOffer;
    }
    
    public async Task Delete(HotelOffer hotel) {
        _context.HotelOffer.Remove(hotel);
        await _context.SaveChangesAsync();
    }
}

