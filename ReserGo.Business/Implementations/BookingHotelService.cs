using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Response;
using ReserGo.Common.Requests.Notification;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingHotelService : IBookingHotelService {
    private readonly ILogger<BookingHotelService> _logger;
    private readonly IHotelOfferService _hotelOfferService;
    private readonly IBookingHotelDataAccess _bookingHotelDataAccess;
    private readonly INotificationService _notificationService;

    public BookingHotelService(ILogger<BookingHotelService> logger,
        IHotelOfferService hotelOfferService,
        IBookingHotelDataAccess bookingHotelDataAccess, INotificationService notificationService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _bookingHotelDataAccess = bookingHotelDataAccess;
        _notificationService = notificationService;
    }

    public async Task<BookingResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user) {
        try {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var hotelOffer = await _hotelOfferService.GetById(request.HotelOfferId);
            if (hotelOffer == null) {
                _logger.LogError("Hotel offer not found for id { id }", request.HotelOfferId);
                throw new InvalidDataException("Hotel offer not found");
            }

            var bookingHotel = new BookingHotel {
                HotelOfferId = hotelOffer.Id,
                UserId = user.UserId,
                BookingDate = request.BookingDate,
                NumberOfGuests = request.NumberOfGuests,
                IsConfirmed = request.IsConfirmed,
                CreatedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Creating booking hotel for user { id }", user.UserId);
            bookingHotel = await _bookingHotelDataAccess.Create(bookingHotel);

            if (bookingHotel == null) throw new InvalidDataException("Booking hotel not created");

            if (hotelOffer.Hotel.Name == null) {
                _logger.LogError("Hotel name is not available for offer {id}", hotelOffer.Id);
                throw new InvalidDataException("Hotel name is not available.");
            }

            var hotelName = hotelOffer.Hotel.Name;
            if (string.IsNullOrEmpty(hotelName)) {
                _logger.LogError("Hotel name is not available for offer {id}", hotelOffer.Id);
                throw new InvalidDataException("Hotel name is not available.");
            }

            // Create a notification
            var notification = new NotificationCreationRequest {
                Title = "New Reservation",
                Message =
                    $"New reservation made by {user.Username} for offer {hotelOffer.OfferTitle} at {hotelOffer.Hotel.Name} " +
                    $"number of guests: {request.NumberOfGuests}",
                Type = "Hotel",
                Name = hotelName,
                UserId = hotelOffer.UserId
            };
            var notificationDto = await _notificationService.CreateNotification(notification);

            return new BookingResponses {
                Notification = notificationDto,
                Booking = bookingHotel.ToDto()
            };
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}