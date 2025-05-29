using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Response;
using ReserGo.Common.Requests.Notification;
using ReserGo.Shared;
using ReserGo.Common.Enum;
using System.Globalization;
namespace ReserGo.Business.Implementations;

public class MetricsService : IMetricsService {
    private readonly ILogger<BookingRestaurantService> _logger;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly IBookingRestaurantDataAccess _bookingRestaurantDataAccess;
    private readonly INotificationService _notificationService;
    private readonly IBookingHotelDataAccess _bookingHotelDataAccess;
    private readonly IBookingEventDataAccess _bookingEventDataAccess;

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

    public async Task<MetricsResponse> GetMetricsMonths(Product product, Guid userId) {
        try {
            if (userId == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            throw new NotImplementedException();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
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
                .Select(b => new { b.BookingDate, b.Price })
                .Concat(restaurantBookings.Select(b => new { b.BookingDate, b.Price }))
                .Concat(eventBookings.Select(b => new { b.BookingDate, b.Price }));

            var currentYear = DateTime.UtcNow.Year;

            var allMonths = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .ToDictionary(m => m, m => 0.0);

            var groupedByMonth = allBookings
                .Where(b => b.BookingDate.Year == currentYear)
                .GroupBy(b => b.BookingDate.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(b => b.Price).GetValueOrDefault()
                );
            
            foreach (var month in allMonths.Keys.ToList()) {
                if (!groupedByMonth.ContainsKey(month)) {
                    groupedByMonth[month] = 0.0;
                }
            }

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
            if (!hotelGrouped.ContainsKey(month)) {
                hotelGrouped[month] = 0;
            }
            if (!restaurantGrouped.ContainsKey(month)) {
                restaurantGrouped[month] = 0;
            }
            if (!occasionGrouped.ContainsKey(month)) {
                occasionGrouped[month] = 0;
            }
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
        var today = DateTime.UtcNow;
        var days30Before = today.AddDays(-30);
        var days60Before = today.AddDays(-60);

        var nbBookingThisMonth = 0;
        var nbBookingLastMonth = 0;


        switch (types) {
            case Product.Hotel:
                nbBookingThisMonth = await _bookingHotelDataAccess.GetNbBookingsLast30Days(adminId);
                nbBookingLastMonth =
                    await _bookingHotelDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days30Before, today);
                break;
            case Product.Restaurant:
                nbBookingThisMonth = await _bookingRestaurantDataAccess.GetNbBookingsLast30Days(adminId);
                nbBookingLastMonth =
                    await _bookingRestaurantDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before,
                        days30Before);
                break;
            case Product.Event:
                nbBookingThisMonth = await _bookingEventDataAccess.GetNbBookingsLast30Days(adminId);
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
        else if (nbBookingThisMonth > 0) {
            statsPercent = 100;
            up = true;
        }

        return new MetricsResponse {
            StatsNumber = nbBookingThisMonth,
            StatsPercent = Math.Round(statsPercent, 2),
            Up = up
        };
    }
}