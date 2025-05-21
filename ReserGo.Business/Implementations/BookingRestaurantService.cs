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

namespace ReserGo.Business.Implementations;

public class BookingRestaurantService : IBookingRestaurantService {
    private readonly ILogger<BookingRestaurantService> _logger;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly IBookingRestaurantDataAccess _bookingRestaurantDataAccess;
    private readonly INotificationService _notificationService;

    public BookingRestaurantService(ILogger<BookingRestaurantService> logger,
        IRestaurantOfferService restaurantOfferService,
        IBookingRestaurantDataAccess bookingRestaurantDataAccess, INotificationService notificationService) {
        _logger = logger;
        _restaurantOfferService = restaurantOfferService;
        _bookingRestaurantDataAccess = bookingRestaurantDataAccess;
        _notificationService = notificationService;
    }

    public async Task<BookingResponses> CreateBooking(BookingRestaurantRequest request, ConnectedUser user) {
        try {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var restaurantOffer = await _restaurantOfferService.GetById(request.RestaurantOfferId);
            if (restaurantOffer == null) {
                _logger.LogError("Restaurant offer not found for id { id }", request.RestaurantOfferId);
                throw new InvalidDataException("Restaurant offer not found");
            }

            var bookingRestaurant = new BookingRestaurant {
                RestaurantOfferId = restaurantOffer.Id,
                UserId = user.UserId,
                BookingDate = request.BookingDate,
                NumberOfGuests = request.NumberOfGuests,
                IsConfirmed = request.IsConfirmed,
                CreatedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Creating booking restaurant for user { id }", user.UserId);
            bookingRestaurant = await _bookingRestaurantDataAccess.Create(bookingRestaurant);

            if (bookingRestaurant == null) throw new InvalidDataException("Booking restaurant not created");

            if (restaurantOffer.Restaurant.Name == null) {
                _logger.LogError("Restaurant name is not available for offer {id}", restaurantOffer.Id);
                throw new InvalidDataException("Restaurant name is not available.");
            }

            var restaurantName = restaurantOffer.Restaurant.Name;
            if (string.IsNullOrEmpty(restaurantName)) {
                _logger.LogError("Restaurant name is not available for offer {id}", restaurantOffer.Id);
                throw new InvalidDataException("Restaurant name is not available.");
            }

            // Create a notification
            var notification = new NotificationCreationRequest {
                Title = "New Reservation",
                Message =
                    $"New reservation made by {user.Username}  at {restaurantOffer.Restaurant.Name} " +
                    $"number of guests: {request.NumberOfGuests}",
                Type = "Restaurant",
                Name = restaurantName,
                UserId = restaurantOffer.UserId
            };
            var notificationDto = await _notificationService.CreateNotification(notification);

            return new BookingResponses {
                Notification = notificationDto,
                Booking = bookingRestaurant.ToDto()
            };
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}