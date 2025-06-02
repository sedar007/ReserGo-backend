using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class LoginDataAccess : ILoginDataAccess {
    private readonly ReserGoContext _context;

    public LoginDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<Login?> GetById(Guid id) {
        return await _context.Login.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Login> Create(Login login) {
        var newData = _context.Login.Add(login);
        await _context.SaveChangesAsync();
        return await GetById(newData.Entity.Id) ?? throw new NullDataException("Error creating user login.");
    }

    public async Task<Login?> GetByUserId(Guid id) {
        return await _context.Login.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task Update(Login login) {
        _context.Login.Update(login);
        await _context.SaveChangesAsync();
    }
}