using System.Globalization;
using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Enum;
using ReserGo.Common.Response;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class MetricsService : IMetricsService {
    private readonly IBookingEventDataAccess _bookingEventDataAccess;
    private readonly IBookingHotelDataAccess _bookingHotelDataAccess;
    private readonly IBookingRestaurantDataAccess _bookingRestaurantDataAccess;
    private readonly ILogger<BookingRestaurantService> _logger;
    private readonly INotificationService _notificationService;
    private readonly IRestaurantOfferService _restaurantOfferService;

    public MetricsService(ILogger<BookingRestaurantService> logger,
        IRestaurantOfferService restaurantOfferService,
        IBookingRestaurantDataAccess bookingRestaurantDataAccess,
        INotificationService notificationService,
        IBookingHotelDataAccess bookingHotelDataAccess,
        IBookingEventDataAccess bookingEventDataAccess) {
        _logger = logger;
        _restaurantOfferService = restaurantOfferService;
        _bookingRestaurantDataAccess = bookingRestaurantDataAccess;
        _notificationService = notificationService;
        _bookingHotelDataAccess = bookingHotelDataAccess;
        _bookingEventDataAccess = bookingEventDataAccess;
    }


    public async Task<Dictionary<string, double>> GetMonthlySales(Guid userId) {
        try {
            if (userId == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var hotelBookings = await _bookingHotelDataAccess.GetBookingYearsByUserId(userId);
            var restaurantBookings = await _bookingRestaurantDataAccess.GetBookingYearsByUserId(userId);
            var eventBookings = await _bookingEventDataAccess.GetBookingYearsByUserId(userId);

            var allBookings = hotelBookings
                .Select(b => new { b.BookingDate, b.PriceTotal })
                .Concat(restaurantBookings.Select(b => new { b.BookingDate, b.PriceTotal }))
                .Concat(eventBookings.Select(b => new { b.BookingDate, b.PriceTotal }));

            var currentYear = DateTime.UtcNow.Year;

            var allMonths = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .ToDictionary(m => m, m => 0.0);

            var groupedByMonth = allBookings
                .Where(b => b.BookingDate.Year == currentYear)
                .GroupBy(b => b.BookingDate.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(
                    g => g.Key,
                    g => Math.Round(g.Sum(b => b.PriceTotal), 2)
                );

            foreach (var month in allMonths.Keys.ToList())
                if (!groupedByMonth.ContainsKey(month))
                    groupedByMonth[month] = 0.0;

            var result = allMonths.Keys.ToDictionary(m => m, m => groupedByMonth[m]);
            return result;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, Dictionary<string, int>>> GetMonthlyBookingsByCategory(Guid userId) {
        try {
            if (userId == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var hotelBookings = await _bookingHotelDataAccess.GetBookingYearsByUserId(userId);
            var restaurantBookings = await _bookingRestaurantDataAccess.GetBookingYearsByUserId(userId);
            var eventBookings = await _bookingEventDataAccess.GetBookingYearsByUserId(userId);

            var currentYear = DateTime.UtcNow.Year;

            var allMonths = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .ToDictionary(m => m, m => 0);

            var hotelGrouped = hotelBookings
                .Where(b => b.BookingDate.Year == currentYear)
                .GroupBy(b => b.BookingDate.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(g => g.Key, g => g.Count());

            var restaurantGrouped = restaurantBookings
                .Where(b => b.BookingDate.Year == currentYear)
                .GroupBy(b => b.BookingDate.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(g => g.Key, g => g.Count());

            var occasionGrouped = eventBookings
                .Where(b => b.BookingDate.Year == currentYear)
                .GroupBy(b => b.BookingDate.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var month in allMonths.Keys.ToList()) {
                if (!hotelGrouped.ContainsKey(month)) hotelGrouped[month] = 0;
                if (!restaurantGrouped.ContainsKey(month)) restaurantGrouped[month] = 0;
                if (!occasionGrouped.ContainsKey(month)) occasionGrouped[month] = 0;
            }

            return new Dictionary<string, Dictionary<string, int>> {
                { "Hotel", allMonths.Keys.ToDictionary(m => m, m => hotelGrouped[m]) },
                { "Restaurant", allMonths.Keys.ToDictionary(m => m, m => restaurantGrouped[m]) },
                { "Event", allMonths.Keys.ToDictionary(m => m, m => occasionGrouped[m]) }
            };
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<MetricsResponse> GetNbBookingsLast30Days(Guid adminId, Product types) {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var days30Before = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var days60Before = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-60));

        var nbBookingThisMonth = 0;
        var nbBookingLastMonth = 0;


        switch (types) {
            case Product.Hotel:
                nbBookingThisMonth =
                    await _bookingHotelDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days30Before, today);
                nbBookingLastMonth =
                    await _bookingHotelDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before,
                        days30Before);
                break;
            case Product.Restaurant:
                nbBookingThisMonth =
                    await _bookingRestaurantDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days30Before, today);
                nbBookingLastMonth =
                    await _bookingRestaurantDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before,
                        days30Before);
                break;
            case Product.Event:
                nbBookingThisMonth =
                    await _bookingEventDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days30Before, today);
                nbBookingLastMonth =
                    await _bookingEventDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before,
                        days30Before);
                break;
            default:
                throw new InvalidDataException("Invalid product type specified.");
        }

        double statsPercent = 0;
        bool? up = null;

        if (nbBookingLastMonth > 0) {
            statsPercent = (double)(nbBookingThisMonth - nbBookingLastMonth) / nbBookingLastMonth * 100;
            up = nbBookingThisMonth > nbBookingLastMonth;
        }
        else if (nbBookingLastMonth == 0 && nbBookingThisMonth > 0) {
            statsPercent = 100;
            up = true;
        }
        else if (nbBookingLastMonth == 0 && nbBookingThisMonth == 0) {
            statsPercent = 0;
            up = null;
        }

        return new MetricsResponse {
            StatsNumber = nbBookingThisMonth,
            StatsPercent = Math.Round(statsPercent, 2),
            Up = up
        };
    }
}