﻿using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared.Exceptions;

namespace ReserGo.DataAccess.Implementations;

public class UserDataAccess : IUserDataAccess {
    private readonly ReserGoContext _context;

    public UserDataAccess(ReserGoContext context) {
        _context = context;
    }

    public async Task<User?> GetById(Guid id) {
        return await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmail(string email) {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByUsername(string username) {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
    }

    public async Task<User> Create(User user) {
        var newData = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return await GetByEmail(newData.Entity.Email) ?? throw new NullDataException("Error creating new user.");
    }

    public async Task Delete(User user) {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> Update(User user) {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<int> SaveChanges() {
        return await _context.SaveChangesAsync();
    }
}