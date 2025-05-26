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

namespace ReserGo.Business.Implementations;

public class MetricsService : IMetricsService {
    private readonly ILogger<BookingRestaurantService> _logger;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly IBookingRestaurantDataAccess _bookingRestaurantDataAccess;
    private readonly INotificationService _notificationService;
    private readonly IBookingHotelDataAccess _bookingHotelDataAccess;
    private readonly IBookingOccasionDataAccess _bookingEventDataAccess;

    public MetricsService(ILogger<BookingRestaurantService> logger,
        IRestaurantOfferService restaurantOfferService,
        IBookingRestaurantDataAccess bookingRestaurantDataAccess, 
        INotificationService notificationService,
        IBookingHotelDataAccess bookingHotelDataAccess,
        IBookingOccasionDataAccess bookingEventDataAccess) {
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
    public async Task<MetricsResponse> GetNbBookingsLast30Days(Guid adminId, Product types) {
        var today = DateTime.UtcNow;
        var days30Before = today.AddDays(-30);
        var days60Before = today.AddDays(-60);

        int nbBookingThisMonth = 0;
        int nbBookingLastMonth = 0;


        switch (types) {
            case Product.Hotel:
                nbBookingThisMonth = await _bookingHotelDataAccess.GetNbBookingsLast30Days(adminId); 
                nbBookingLastMonth = await _bookingHotelDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days30Before, today);
                break;
            case Product.Restaurant:
                nbBookingThisMonth = await _bookingRestaurantDataAccess.GetNbBookingsLast30Days(adminId);
                nbBookingLastMonth = await _bookingRestaurantDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before, days30Before);
                break;
            case Product.Occasion:
                nbBookingThisMonth = await _bookingEventDataAccess.GetNbBookingsLast30Days(adminId);
                nbBookingLastMonth = await _bookingEventDataAccess.GetNbBookingBetween2DatesByAdminId(adminId, days60Before, days30Before);
                break;
            default:
                throw new InvalidDataException("Invalid product type specified.");
        }

        double statsPercent = 0;
        bool? up = null;

        if (nbBookingLastMonth > 0) {
            statsPercent = ((double)(nbBookingThisMonth - nbBookingLastMonth) / nbBookingLastMonth) * 100;
            up = nbBookingThisMonth > nbBookingLastMonth;
        } else if (nbBookingThisMonth > 0) {
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