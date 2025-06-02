using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Response;

namespace ReserGo.Business.Implementations;

public class BookingService : IBookingService {
    private readonly IBookingEventService _bookingEventService;
    private readonly IBookingHotelService _bookingHotelService;
    private readonly IBookingRestaurantService _bookingRestaurantService;

    private readonly ILogger<BookingService> _logger;

    public BookingService(ILogger<BookingService> logger,
        IBookingEventService bookingEventService,
        IBookingHotelService bookingHotelService,
        IBookingRestaurantService bookingRestaurantService) {
        _logger = logger;
        _bookingEventService = bookingEventService;
        _bookingHotelService = bookingHotelService;
        _bookingRestaurantService = bookingRestaurantService;
    }


    public async Task<IEnumerable<BookingAllResponses>> GetBookingsByUserId(Guid adminId) {
        if (adminId == Guid.Empty) {
            _logger.LogError("Admin ID is empty");
            throw new InvalidDataException("Admin ID cannot be empty");
        }


        var bookingsHotel = (await _bookingHotelService.GetBookingsByUserId(adminId)).ToList();
        var bookingsEvent = (await _bookingEventService.GetBookingsByUserId(adminId)).ToList();
        var bookingsRestaurant = (await _bookingRestaurantService.GetBookingsByUserId(adminId)).ToList();
        if (!bookingsHotel.Any() && !bookingsEvent.Any() && !bookingsRestaurant.Any()) {
            _logger.LogWarning("No bookings found for the given admin ID");
            return Enumerable.Empty<BookingAllResponses>();
        }

        var bookings = bookingsHotel.Concat(bookingsEvent).Concat(bookingsRestaurant)
            .OrderByDescending(b => b.StartDate);
        return bookings;
    }
}