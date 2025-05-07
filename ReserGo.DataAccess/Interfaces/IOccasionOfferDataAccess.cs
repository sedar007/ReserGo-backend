using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IOccasionOfferDataAccess {
    Task<OccasionOffer?> GetById(Guid id);
    Task<OccasionOffer> Create(OccasionOffer occasionOffer);
    Task<OccasionOffer> Update(OccasionOffer occasionOffer);
    Task<IEnumerable<OccasionOffer>> GetOccasionsOfferByUserId(Guid userId);
    Task Delete(OccasionOffer occasionOffer);
}