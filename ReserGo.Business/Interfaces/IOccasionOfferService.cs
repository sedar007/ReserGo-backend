using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Occasion;

namespace ReserGo.Business.Interfaces;

public interface IOccasionOfferService {
    Task<OccasionOfferDto?> GetById(Guid id);
    Task<OccasionOfferDto> Create(OccasionOfferCreationRequest request);
    Task<OccasionOfferDto> Update(Guid id, OccasionOfferUpdateRequest request);
    Task<IEnumerable<OccasionOfferDto>> GetOccasionsByUserId(Guid userId);
    Task Delete(Guid id);
}