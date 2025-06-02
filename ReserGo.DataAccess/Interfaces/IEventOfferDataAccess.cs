using ReserGo.Common.Entity;
using ReserGo.Common.Requests.Products.Event;

namespace ReserGo.DataAccess.Interfaces;

public interface IEventOfferDataAccess {
    Task<EventOffer?> GetById(Guid id);
    Task<EventOffer> Create(EventOffer eventOffer);
    Task<EventOffer> Update(EventOffer eventOffer);
    Task<IEnumerable<EventOffer>> GetEventsOfferByUserId(Guid userId);
    Task<IEnumerable<EventOffer>> SearchAvailability(EventSearchAvailabilityRequest request);

    Task Delete(EventOffer eventOffer);
}