using ReserGo.Common.Entity;

namespace ReserGo.DataAccess.Interfaces;

public interface IEventOfferDataAccess {
    Task<EventOffer?> GetById(Guid id);
    Task<EventOffer> Create(EventOffer eventOffer);
    Task<EventOffer> Update(EventOffer eventOffer);
    Task<IEnumerable<EventOffer>> GetEventsOfferByUserId(Guid userId);
    Task Delete(EventOffer eventOffer);
}