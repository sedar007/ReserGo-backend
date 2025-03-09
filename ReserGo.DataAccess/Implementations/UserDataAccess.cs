using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Entity;
using ReserGo.Common.Requests;

using Microsoft.EntityFrameworkCore;
namespace ReserGo.DataAccess.Implementations {
    public class UserDataAccess : IUserDataAccess {
        private readonly ReserGoContext _context;
        public UserDataAccess(ReserGoContext context) {
            _context = context;
        }
        
        public async Task<User?> GetByUsername(string username) {
            return await _context.User.FirstOrDefaultAsync(x => x.Username == username);
        }
        
        public async Task<User?> GetByEmail(string email) {
            return await _context.User.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task SaveChanges() {
            return _context.SaveChangesAsync();
        }
        
    }
}
