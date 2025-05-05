using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Occasion;

namespace ReserGo.Business.Interfaces;

public interface IOccasionOfferService {
    Task<OccasionOfferDto?> GetById(int id);
    Task<OccasionOfferDto> Create(OccasionOfferCreationRequest request);
    Task<OccasionOfferDto> Update(int id, OccasionOfferUpdateRequest request);
    Task<IEnumerable<OccasionOfferDto>> GetOccasionsByUserId(int userId);
    Task Delete(int id);
}