using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Event;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Response;
using ReserGo.Common.Requests.Notification;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingEventService : IBookingEventService {
    private readonly ILogger<BookingEventService> _logger;
    private readonly IEventOfferService _hotelOfferService;
    private readonly IBookingEventDataAccess _bookingEventDataAccess;
    private readonly INotificationService _notificationService;
    private readonly IRoomAvailabilityService _roomAvailabilityService;
    private readonly IEventOfferService _eventOfferService;

    public BookingEventService(ILogger<BookingEventService> logger,
        IEventOfferService hotelOfferService,
        IBookingEventDataAccess bookingEventDataAccess, INotificationService notificationService,
        IRoomAvailabilityService roomAvailabilityService,
        IEventOfferService eventOfferService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _bookingEventDataAccess = bookingEventDataAccess;
        _notificationService = notificationService;
        _roomAvailabilityService = roomAvailabilityService;
        _eventOfferService = eventOfferService;
    }

    public async Task<BookingResponses> CreateBooking(BookingEventRequest request, ConnectedUser user) {
           try {
               if (user == null) {
                   _logger.LogError("User not found");
                   throw new InvalidDataException(Consts.UserNotFound);
               }

               var eventOffer = await _eventOfferService.GetById(request.EventOfferId);
               if (eventOffer == null) {
                   _logger.LogError("Event offer not found for id { id }", request.EventOfferId);
                   throw new InvalidDataException("Event offer not found");
               }
              
               if (request.NumberOfGuests > eventOffer.GuestLimit) {
                   _logger.LogError("Booking exceeds the remaining capacity for offer { id }", eventOffer.Id);
                   throw new InvalidDataException($"Cannot book {request.NumberOfGuests} guests.");
               }
               
               var numberDays = (request.EndDate.ToDateTime(TimeOnly.MinValue) - request.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
               var priceTotal = numberDays * eventOffer.PricePerDay;
               var bookingEvent = new BookingEvent {
                   EventOfferId = eventOffer.Id,
                   EventId = eventOffer.Event.Id,
                   UserId = user.UserId,
                   StartDate = request.StartDate,
                     EndDate = request.EndDate,
                   PricePerPerson =  eventOffer.PricePerDay,
                   PriceTotal = priceTotal,
                   NumberOfGuests = request.NumberOfGuests,
                   IsConfirmed = request.IsConfirmed,
                   BookingDate = DateTime.UtcNow
               };
               
               _logger.LogInformation("Creating booking event for user { id }", user.UserId);
               bookingEvent = await _bookingEventDataAccess.Create(bookingEvent);

               if (bookingEvent == null) throw new InvalidDataException("Booking event not created");

               if (eventOffer.Event.Name == null) {
                   _logger.LogError("Event name is not available for offer {id}", eventOffer.Id);
                   throw new InvalidDataException("Event name is not available.");
               }

               var eventName = eventOffer.Event.Name;
               if (string.IsNullOrEmpty(eventName)) {
                   _logger.LogError("Event name is not available for offer {id}", eventOffer.Id);
                   throw new InvalidDataException("Event name is not available.");
               }

               // Create a notification
               var notification = new NotificationCreationRequest {
                   Title = "New Reservation",
                   Message =
                       $"New reservation made by {user.Username}  at {eventOffer.Event.Name} " +
                       $"number of guests: {request.NumberOfGuests}",
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

   /* public async Task<IEnumerable<BookingEventDto>> GetBookingsByUserId(Guid userId) {
        var bookings = await _bookingEventDataAccess.GetBookingsByUserId(userId);
        return bookings.Select(b => new BookingEventDto {
            Id = b.Id,
            EventId = b.EventId,
            UserId = b.UserId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            NumberOfGuests = b.NumberOfGuests,
            IsConfirmed = b.IsConfirmed,
            BookingDate = b.BookingDate
        });
    }*/

    public async Task<IEnumerable<BookingEventDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingEventDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }
}