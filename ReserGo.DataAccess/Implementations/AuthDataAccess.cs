/*using Microsoft.EntityFrameworkCore;
using ReserGo.Common.Entity;
using ReserGo.DataAccess.Interfaces;

namespace ReserGo.DataAccess.Implementations {
    public class AuthDataAccess : IAuthDataAccess {

        private readonly ReserGoContext _context;
        public AuthDataAccess(ReserGoContext context)
        {
            _context = context;
        }

        public async Task<Login> Create(Login login) {
            var newData = _context.Login.Add(login);
            await _context.SaveChangesAsync();
            return await GetUserById(newData.Entity.Id) ?? throw new NullReferenceException("Erreur lors de la creation de l'utilisateur.");
        }
    }
} */

