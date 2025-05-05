using ReserGo.Common.DTO;
using ReserGo.Common.Requests.Products.Occasion;

namespace ReserGo.Business.Interfaces;

public interface IOccasionService {
    Task<OccasionDto?> GetById(int id);
    Task<OccasionDto?> GetByStayId(long stayId);
    Task<IEnumerable<OccasionDto>> GetOccasionsByUserId(int userId);
    Task<OccasionDto> Create(OccasionCreationRequest request);
    Task<OccasionDto> Update(long stayId, OccasionUpdateRequest request);
    Task Delete(int id);
}