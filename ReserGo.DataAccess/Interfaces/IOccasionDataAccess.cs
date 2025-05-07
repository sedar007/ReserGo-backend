using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IOccasionDataAccess {
    Task<Occasion?> GetById(Guid id);
    Task<Occasion?> GetByStayId(long stayId);
    Task<IEnumerable<Occasion>> GetOccasionsByUserId(Guid userId);
    Task<Occasion> Create(Occasion occasion);
    Task<Occasion> Update(Occasion occasion);
    Task Delete(Occasion occasion);
}