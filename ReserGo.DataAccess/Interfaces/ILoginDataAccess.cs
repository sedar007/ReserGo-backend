using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces {
    public interface ILoginDataAccess {
        Task<Login?> GetByUserId(int userId);
    }
}
