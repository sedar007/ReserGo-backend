using ReserGo.Common.Response;
using ReserGo.Common.Enum;

namespace ReserGo.Business.Interfaces;

public interface IMetricsService {
    Task<MetricsResponse> GetMetricsMonths(Product product, Guid userId);
    Task<MetricsResponse> GetNbBookingsLast30Days(Guid adminId, Product types);

    
}