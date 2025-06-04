using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;
using System.Globalization;
using System.Text;

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

    public async Task<RestaurantOffer> Create(RestaurantOffer restaurantOffer) {
        var newData = _context.RestaurantOffer.Add(restaurantOffer);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ??
               throw new NullDataException("Error creating new restaurant offer.");
    }

    public async Task<RestaurantOffer> Update(RestaurantOffer restaurantOffer) {
        var data = _context.RestaurantOffer.Update(restaurantOffer);
        await _context.SaveChangesAsync();
        return await GetById(data.Entity.Id) ??
               throw new NullDataException("Error updating restaurant offer.");
    }

    public async Task Delete(RestaurantOffer restaurantOffer) {
        _context.RestaurantOffer.Remove(restaurantOffer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RestaurantOffer>> SearchAvailability(RestaurantSearchAvailabilityRequest request) {
        string Normalize(string input) => string.IsNullOrEmpty(input)
            ? string.Empty
            : new string(input.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .ToLower();

        var normalizedRequestCuisine = Normalize(request.CuisineType ?? string.Empty);

        var query = _context.RestaurantOffer
            .Include(o => o.Restaurant)
            .Where(o => o.OfferStartDate <= request.Date &&
                        o.OfferEndDate >= request.Date &&
                        o.GuestLimit - o.GuestNumber >= request.NumberOfGuests);

        var result = await query.ToListAsync();

        if (!string.IsNullOrEmpty(request.CuisineType))
        {
            result = result.Where(o => !string.IsNullOrEmpty(o.Restaurant.CuisineType) &&
                (Normalize(o.Restaurant.CuisineType).Contains(normalizedRequestCuisine) ||
                 normalizedRequestCuisine.Contains(Normalize(o.Restaurant.CuisineType)))).ToList();
        }

        return result;
    }
}
