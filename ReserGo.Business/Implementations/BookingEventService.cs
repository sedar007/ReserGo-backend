using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Notification;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingEventService : IBookingEventService {
    private readonly IBookingEventDataAccess _bookingEventDataAccess;
    private readonly IEventOfferService _eventOfferService;
    private readonly ILogger<BookingEventService> _logger;
    private readonly INotificationService _notificationService;
    private readonly IImageService _imageService;

    public BookingEventService(ILogger<BookingEventService> logger,
        IBookingEventDataAccess bookingEventDataAccess, INotificationService notificationService,
        IEventOfferService eventOfferService, IImageService imageService) {
        _logger = logger;
        _bookingEventDataAccess = bookingEventDataAccess;
        _notificationService = notificationService;
        _eventOfferService = eventOfferService;
        _imageService = imageService;
    }

    public async Task<BookingResponses> CreateBooking(BookingEventRequest request, ConnectedUser user) {
        try {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var eventOffer = await _eventOfferService.GetById(request.EventOfferId);
            if (eventOffer == null) {
                _logger.LogError("Event offer not found for id {Id}", request.EventOfferId);
                throw new InvalidDataException("Event offer not found");
            }
            

            var numberDays = (request.EndDate.ToDateTime(TimeOnly.MinValue) -
                              request.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
            numberDays = numberDays < 1 ? 1 : numberDays;
            var priceTotal = numberDays * eventOffer.PricePerDay;
            var bookingEvent = new BookingEvent {
                EventOfferId = eventOffer.Id,
                EventId = eventOffer.Event.Id,
                UserId = user.UserId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PricePerDay = eventOffer.PricePerDay,
                PriceTotal = priceTotal,
                NumberOfGuests = eventOffer.Event.Capacity,
                IsConfirmed = true,
                BookingDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _logger.LogInformation("Creating booking event for user {Id}", user.UserId);
            bookingEvent = await _bookingEventDataAccess.Create(bookingEvent);

            if (bookingEvent == null) throw new InvalidDataException("Booking event not created");

            if (eventOffer.Event.Name == null) {
                _logger.LogError("Event name is not available for offer {Id}", eventOffer.Id);
                throw new InvalidDataException("Event name is not available.");
            }

            var eventName = eventOffer.Event.Name;
            if (string.IsNullOrEmpty(eventName)) {
                _logger.LogError("Event name is not available for offer {Id}", eventOffer.Id);
                throw new InvalidDataException("Event name is not available.");
            }

            // Create a notification
            var notification = new NotificationCreationRequest {
                Title = "New Reservation",
                Message =
                    $"New reservation made by {user.Username}  at {eventOffer.Event.Name} ",
                Type = "Event",
                Name = eventName,
                UserId = eventOffer.UserId
            };
            var notificationDto = await _notificationService.CreateNotification(notification);

            return new BookingResponses {
                Notification = notificationDto,
                Booking = bookingEvent.ToDto()
            };
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<BookingEventDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingEventDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }

    public async Task<IEnumerable<BookingAllResponses>> GetBookingsByUserId(Guid userId) {
        var bookings = await _bookingEventDataAccess.GetBookingsByUserId(userId);
        return await Task.WhenAll(bookings
            .GroupBy(b => new { b.EventId, b.StartDate, b.EndDate })
            .Select(async group => new BookingAllResponses {
                Name = group.First().Event.Name,
                Type = "Event",
                ImageSrc = await _imageService.GetPicture(group.First().Event.Picture ?? "default-event.png"),
                NbGuest = group.Sum(b => b.NumberOfGuests),
                TotalPrice = group.Sum(b => b.PriceTotal),
                StartDate = group.Key.StartDate,
                EndDate = group.Key.EndDate
            }));
    }
}