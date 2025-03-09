using ReserGo.DataAccess.Interfaces;


namespace ReserGo.DataAccess.Implementations {
    public class AuthDataAccess : IAuthDataAccess {

        private readonly ReserGoContext _context;
        public AuthDataAccess(ReserGoContext context)
        {
            _context = context;
        }
    }
}
