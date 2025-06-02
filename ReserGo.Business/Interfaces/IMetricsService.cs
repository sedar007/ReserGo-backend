using ReserGo.Common.Enum;
using ReserGo.Common.Response;

namespace ReserGo.Business.Interfaces;

public interface IMetricsService {
    Task<MetricsResponse> GetNbBookingsLast30Days(Guid adminId, Product types);
    Task<Dictionary<string, double>> GetMonthlySales(Guid userId);
    Task<Dictionary<string, Dictionary<string, int>>> GetMonthlyBookingsByCategory(Guid userId);
}