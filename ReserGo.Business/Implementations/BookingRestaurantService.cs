using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Notification;
using ReserGo.Common.Requests.Products.Restaurant;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingRestaurantService : IBookingRestaurantService {
    private readonly IBookingRestaurantDataAccess _bookingRestaurantDataAccess;
    private readonly ILogger<BookingRestaurantService> _logger;
    private readonly INotificationService _notificationService;
    private readonly IRestaurantOfferService _restaurantOfferService;
    private readonly IImageService _imageService;

    public BookingRestaurantService(ILogger<BookingRestaurantService> logger,
        IRestaurantOfferService restaurantOfferService,
        IBookingRestaurantDataAccess bookingRestaurantDataAccess, INotificationService notificationService,
        IImageService imageService) {
        _logger = logger;
        _restaurantOfferService = restaurantOfferService;
        _bookingRestaurantDataAccess = bookingRestaurantDataAccess;
        _notificationService = notificationService;
        _imageService = imageService;
    }

    public async Task<BookingResponses> CreateBooking(BookingRestaurantRequest request, ConnectedUser user) {
        try {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var restaurantOffer = await _restaurantOfferService.GetById(request.RestaurantOfferId);
            if (restaurantOffer == null) {
                _logger.LogError("Restaurant offer not found for id {Id}", request.RestaurantOfferId);
                throw new InvalidDataException("Restaurant offer not found");
            }

            var remainingCapacity = restaurantOffer.GuestLimit - restaurantOffer.GuestNumber;
            if (request.NumberOfGuests > remainingCapacity) {
                _logger.LogError("Booking exceeds the remaining capacity for offer {Id}", restaurantOffer.Id);
                throw new InvalidDataException(
                    $"Cannot book {request.NumberOfGuests} guests. Only {remainingCapacity} spots are available.");
            }


            var priceTotal = request.NumberOfGuests * restaurantOffer.PricePerPerson;
            var bookingRestaurant = new BookingRestaurant {
                RestaurantOfferId = restaurantOffer.Id,
                RestaurantId = restaurantOffer.Restaurant.Id,
                UserId = user.UserId,
                Date = request.Date,
                PricePerPerson = restaurantOffer.PricePerPerson,
                PriceTotal = priceTotal,
                NumberOfGuests = request.NumberOfGuests,
                IsConfirmed = true,
                BookingDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _logger.LogInformation("Creating booking restaurant for user {Id}", user.UserId);
            bookingRestaurant = await _bookingRestaurantDataAccess.Create(bookingRestaurant);

            if (bookingRestaurant == null) throw new InvalidDataException("Booking restaurant not created");
            restaurantOffer.GuestNumber += request.NumberOfGuests;
            restaurantOffer = await _restaurantOfferService.Update(restaurantOffer);
            if (restaurantOffer.Restaurant.Name == null) {
                _logger.LogError("Restaurant name is not available for offer {Id}", restaurantOffer.Id);
                throw new InvalidDataException("Restaurant name is not available.");
            }

            var restaurantName = restaurantOffer.Restaurant.Name;
            if (string.IsNullOrEmpty(restaurantName)) {
                _logger.LogError("Restaurant name is not available for offer {Id}", restaurantOffer.Id);
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

    public async Task<IEnumerable<BookingAllResponses>> GetBookingsByUserId(Guid userId) {
        var bookings = await _bookingRestaurantDataAccess.GetBookingsByUserId(userId);
        return await Task.WhenAll(bookings
            .GroupBy(b => new { b.RestaurantId, b.Date })
            .Select(async group => new BookingAllResponses {
                Name = group.First().Restaurant.Name,
                Type = "Restaurant",
                ImageSrc = await _imageService.GetPicture(group.First().Restaurant.Picture ?? "default-event.png"),
                NbGuest = group.Sum(b => b.NumberOfGuests),
                TotalPrice = group.Sum(b => b.PriceTotal),
                StartDate = group.Key.Date,
                EndDate = group.Key.Date
            }));
    }

    public async Task<IEnumerable<BookingRestaurantDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingRestaurantDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }
}