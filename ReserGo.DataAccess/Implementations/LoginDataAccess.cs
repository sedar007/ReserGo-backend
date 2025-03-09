using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Entity;

using Microsoft.EntityFrameworkCore;
namespace ReserGo.DataAccess.Implementations {
    public class LoginDataAccess : ILoginDataAccess {
        private readonly ReserGoContext _context;
        public LoginDataAccess(ReserGoContext context) {
            _context = context;
        }
        
        public async Task<Login?> GetByUserId(int userId) {
            return await _context.Login.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        
    }
}
