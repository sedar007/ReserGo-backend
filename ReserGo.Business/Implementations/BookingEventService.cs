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

    public BookingEventService(ILogger<BookingEventService> logger,
        IEventOfferService hotelOfferService,
        IBookingEventDataAccess bookingEventDataAccess, INotificationService notificationService,
        IRoomAvailabilityService roomAvailabilityService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _bookingEventDataAccess = bookingEventDataAccess;
        _notificationService = notificationService;
        _roomAvailabilityService = roomAvailabilityService;
    }

    /* public async Task<BookingResponses> CreateBooking(BookingEventRequest request, ConnectedUser user) {
     try {
         if (user == null) {
             _logger.LogError("User not found");
             throw new InvalidDataException(Consts.UserNotFound);
         }

         var availability = await _roomAvailabilityService.GetAvailabilityByRoomId(request.RoomId);

         if (availability == null || availability.StartDate.Date > request.StartDate.Date || availability.EndDate.Date < request.EndDate.Date) {
             throw new InvalidDataException("The room is not available for the selected dates.");
         }

         var existingBookings = await _bookingEventDataAccess.GetBookingsByRoomId(request.RoomId);
         if (existingBookings.Any(b =>
                 request.StartDate.Date < b.EndDate.Date && request.EndDate.Date > b.StartDate.Date)) {
             throw new InvalidDataException("The room is already booked for the selected dates.");
         }

         var reservation = new BookingEvent {
             RoomId = request.RoomId,
             EventId = availability.Event.Id,
             UserId = request.UserId,
             StartDate = request.StartDate,
             BookingDate = DateTime.UtcNow,
             EndDate = request.EndDate,
             NumberOfGuests = request.NumberOfGuests,
             IsConfirmed = request.IsConfirmed,
             CreatedAt = DateTime.UtcNow
         };

         var createdReservation = await _bookingEventDataAccess.Create(reservation);
         if (createdReservation == null) {
             _logger.LogError("Booking hotel not created");
             throw new InvalidDataException("Booking hotel not created");
         }
         var bookingEvent = createdReservation?.ToDto();

         var notification = new NotificationCreationRequest {
             Title = "New Reservation",
             Message = $"New reservation made by {user.Username} for offer at {availability.Event.Name} " +
                       $"number of guests: {request.NumberOfGuests}",
             Type = "Event",
             Name = availability.Event.Name,
             UserId = availability.Event.UserId,
         };
         var notificationDto = await _notificationService.CreateNotification(notification);

         return new BookingResponses {
             Notification = notificationDto,
             Booking = bookingEvent
         };
     } catch (Exception e) {
         Console.WriteLine(e);
         throw;
     }
 }*/

    public async Task<IEnumerable<BookingEventDto>> GetBookingsByUserId(Guid userId) {
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
    }

    public async Task<IEnumerable<BookingEventDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingEventDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }
}