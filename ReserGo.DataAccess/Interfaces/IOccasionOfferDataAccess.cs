using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IOccasionOfferDataAccess {
    Task<OccasionOffer?> GetById(int id);
    Task<OccasionOffer> Create(OccasionOffer occasionOffer);
    Task<OccasionOffer> Update(OccasionOffer occasionOffer);
    Task<IEnumerable<OccasionOffer>> GetOccasionsOfferByUserId(int userId);
    Task Delete(OccasionOffer occasionOffer);
}