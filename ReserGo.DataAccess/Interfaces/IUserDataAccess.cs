using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IUserDataAccess {
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUsername(string username);
    Task<User> Create(User user);
    Task<User?> GetById(int id);
    Task Delete(User user);
    Task<User> Update(User user);
}

