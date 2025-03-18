using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface ILoginDataAccess {
    Task<Login?> GetById(int id);
    Task<Login> Create(Login login);
}

