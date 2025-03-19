using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Entity;
using Microsoft.EntityFrameworkCore;

namespace ReserGo.DataAccess.Implementations;

public class LoginDataAccess : ILoginDataAccess {
    
    private readonly ReserGoContext _context;
    
    public LoginDataAccess(ReserGoContext context) {
        _context = context;
    }
    
    public async Task<Login?> GetById(int id) {
        return await _context.Login.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Login> Create(Login login) {
        var newData = _context.Login.Add(login);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ?? throw new NullReferenceException("Error creating user login.");
    }
    
    public async Task<Login?> GetByUserId(int id) {
        return await _context.Login.FirstOrDefaultAsync(x => x.UserId == id);
    }
}

