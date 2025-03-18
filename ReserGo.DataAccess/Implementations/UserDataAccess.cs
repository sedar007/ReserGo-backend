using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations;

public class UserDataAccess : IUserDataAccess {
    
    private readonly ReserGoContext _context;
    
    public UserDataAccess(ReserGoContext context) {
        _context = context;
    }
    
    public async Task<User?> GetById(int id) {
        return await _context.User.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmail(string email) {
        return await _context.User.FirstOrDefaultAsync(x => x.Email == email);
    }
    
    public async Task<User?> GetByUsername(string username) {
        return await _context.User.FirstOrDefaultAsync(x => x.Username == username);
    }
    
    public async Task<User> Create(User user) {
        EntityEntry<User> newData = _context.User.Add(user);
        await _context.SaveChangesAsync();
        return await GetByEmail(newData.Entity.Email) ?? throw new NullReferenceException("Error creating new user.");
    }
    
    public async Task Delete(User user) {
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> Update(User user) {
        _context.User.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<int> SaveChanges() {
        return await _context.SaveChangesAsync();
    }
}

