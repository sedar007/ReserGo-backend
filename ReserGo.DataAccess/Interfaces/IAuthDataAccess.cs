using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces {
    public interface IAuthDataAccess {
        Task<Login?> GetUserById(int id);
        Task<Login> Create(Login login);
    }
}
