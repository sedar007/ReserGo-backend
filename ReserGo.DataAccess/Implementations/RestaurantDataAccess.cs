using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class RestaurantDataAccess : IRestaurantDataAccess {
    private readonly ReserGoContext _context;

    public RestaurantDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Restaurant?> GetById(Guid id) {
        return await _context.Restaurant.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Restaurant?> GetByStayId(long stayId) {
        return await _context.Restaurant.FirstOrDefaultAsync(x => x.StayId == stayId);
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsByUserId(Guid userId) {
        return await _context.Restaurant.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Restaurant> Create(Restaurant restaurant) {
        var newData = _context.Restaurant.Add(restaurant);
        await _context.SaveChangesAsync();
        return await GetByStayId(newData.Entity.StayId) ??
               throw new NullDataException("Error creating new Restaurant.");
    }

    public async Task<Restaurant> Update(Restaurant restaurant) {
        _context.Restaurant.Update(restaurant);
        await _context.SaveChangesAsync();
        return restaurant;
    }

    public async Task Delete(Restaurant restaurant) {
        _context.Restaurant.Remove(restaurant);
        await _context.SaveChangesAsync();
    }
}