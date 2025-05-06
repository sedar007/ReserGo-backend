using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class RestaurantOfferDataAccess : IRestaurantOfferDataAccess {
    private readonly ReserGoContext _context;

    public RestaurantOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<RestaurantOffer?> GetById(int id) {
        return await _context.RestaurantOffer.Include(h => h.Restaurant).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<RestaurantOffer>> GetRestaurantsOfferByUserId(int userId) {
        return await _context.RestaurantOffer.Where(x => x.UserId == userId).Include(h => h.Restaurant).ToListAsync();
    }

    public async Task<RestaurantOffer> Create(RestaurantOffer user) {
        EntityEntry<RestaurantOffer> newData = _context.RestaurantOffer.Add(user);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullReferenceException("Error creating new restaurant offer.");
    }

    public async Task<RestaurantOffer> Update(RestaurantOffer restaurantOffer) {
        _context.RestaurantOffer.Update(restaurantOffer);
        await _context.SaveChangesAsync();
        return restaurantOffer;
    }

    public async Task Delete(RestaurantOffer restaurant) {
        _context.RestaurantOffer.Remove(restaurant);
        await _context.SaveChangesAsync();
    }
}