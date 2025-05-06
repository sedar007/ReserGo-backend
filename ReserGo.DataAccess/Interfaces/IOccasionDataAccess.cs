using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IOccasionDataAccess {
    Task<Occasion?> GetById(int id);
    Task<Occasion?> GetByStayId(long stayId);
    Task<IEnumerable<Occasion>> GetOccasionsByUserId(int userId);
    Task<Occasion> Create(Occasion occasion);
    Task<Occasion> Update(Occasion occasion);
    Task Delete(Occasion occasion);
}