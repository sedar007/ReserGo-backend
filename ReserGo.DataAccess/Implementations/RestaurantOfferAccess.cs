using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;

namespace ReserGo.DataAccess.Implementations;

public class RestaurantOfferDataAccess : IRestaurantOfferDataAccess {
    private readonly ReserGoContext _context;

    public RestaurantOfferDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<RestaurantOffer?> GetById(Guid id) {
        return await _context.RestaurantOffer
            .Include(h => h.Restaurant)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<RestaurantOffer>> GetRestaurantsOfferByUserId(Guid userId) {
        return await _context.RestaurantOffer.Where(x => x.UserId == userId).Include(h => h.Restaurant).ToListAsync();
    }

    public async Task<RestaurantOffer> Create(RestaurantOffer user) {
        var newData = _context.RestaurantOffer.Add(user);
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
    
    public async Task<IEnumerable<RestaurantOffer>> SearchAvailability(RestaurantSearchAvailabilityRequest request) {
        return await _context.RestaurantOffer
            .Include(o => o.Restaurant)
            .Where(o => o.OfferStartDate <= request.Date &&
                        o.OfferEndDate >= request.Date &&
                        o.GuestLimit - o.GuestNumber >= request.NumberOfGuests &&
                        (string.IsNullOrEmpty(request.CuisineType) ||
                         (o.Restaurant.CuisineType != null && 
                          o.Restaurant.CuisineType.ToLower().Contains(request.CuisineType.ToLower()))))
            .ToListAsync();
    }
    
    
 
}