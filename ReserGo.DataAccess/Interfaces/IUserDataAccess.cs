using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces {
    public interface IUserDataAccess {
     
        Task<User?> GetByUsername(string username);
        Task<User?> GetByEmail(string email);
        
       
    }
}
