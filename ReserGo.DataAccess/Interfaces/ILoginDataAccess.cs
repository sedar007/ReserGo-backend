using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface ILoginDataAccess {
    Task<Login?> GetById(Guid id);
    Task<Login> Create(Login login);
    Task<Login?> GetByUserId(Guid id);

    Task Update(Login login);
}