using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Products.Occasion;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Common.Response;
using ReserGo.Common.Requests.Notification;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingOccasionService : IBookingOccasionService {
    private readonly ILogger<BookingOccasionService> _logger;
    private readonly IOccasionOfferService _hotelOfferService;
    private readonly IBookingOccasionDataAccess _bookingOccasionDataAccess;
    private readonly INotificationService _notificationService;
    private readonly IRoomAvailabilityService _roomAvailabilityService;

    public BookingOccasionService(ILogger<BookingOccasionService> logger,
        IOccasionOfferService hotelOfferService,
        IBookingOccasionDataAccess bookingOccasionDataAccess, INotificationService notificationService,
        IRoomAvailabilityService roomAvailabilityService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _bookingOccasionDataAccess = bookingOccasionDataAccess;
        _notificationService = notificationService;
        _roomAvailabilityService = roomAvailabilityService;
    }

    /* public async Task<BookingResponses> CreateBooking(BookingOccasionRequest request, ConnectedUser user) {
     try {
         if (user == null) {
             _logger.LogError("User not found");
             throw new InvalidDataException(Consts.UserNotFound);
         }

         var availability = await _roomAvailabilityService.GetAvailabilityByRoomId(request.RoomId);

         if (availability == null || availability.StartDate.Date > request.StartDate.Date || availability.EndDate.Date < request.EndDate.Date) {
             throw new InvalidDataException("The room is not available for the selected dates.");
         }

         var existingBookings = await _bookingOccasionDataAccess.GetBookingsByRoomId(request.RoomId);
         if (existingBookings.Any(b =>
                 request.StartDate.Date < b.EndDate.Date && request.EndDate.Date > b.StartDate.Date)) {
             throw new InvalidDataException("The room is already booked for the selected dates.");
         }

         var reservation = new BookingOccasion {
             RoomId = request.RoomId,
             OccasionId = availability.Occasion.Id,
             UserId = request.UserId,
             StartDate = request.StartDate,
             BookingDate = DateTime.UtcNow,
             EndDate = request.EndDate,
             NumberOfGuests = request.NumberOfGuests,
             IsConfirmed = request.IsConfirmed,
             CreatedAt = DateTime.UtcNow
         };

         var createdReservation = await _bookingOccasionDataAccess.Create(reservation);
         if (createdReservation == null) {
             _logger.LogError("Booking hotel not created");
             throw new InvalidDataException("Booking hotel not created");
         }
         var bookingOccasion = createdReservation?.ToDto();

         var notification = new NotificationCreationRequest {
             Title = "New Reservation",
             Message = $"New reservation made by {user.Username} for offer at {availability.Occasion.Name} " +
                       $"number of guests: {request.NumberOfGuests}",
             Type = "Occasion",
             Name = availability.Occasion.Name,
             UserId = availability.Occasion.UserId,
         };
         var notificationDto = await _notificationService.CreateNotification(notification);

         return new BookingResponses {
             Notification = notificationDto,
             Booking = bookingOccasion
         };
     } catch (Exception e) {
         Console.WriteLine(e);
         throw;
     }
 }*/

    public async Task<IEnumerable<BookingOccasionDto>> GetBookingsByUserId(Guid userId) {
        var bookings = await _bookingOccasionDataAccess.GetBookingsByUserId(userId);
        return bookings.Select(b => new BookingOccasionDto {
            Id = b.Id,
            OccasionId = b.OccasionId,
            UserId = b.UserId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            NumberOfGuests = b.NumberOfGuests,
            IsConfirmed = b.IsConfirmed,
            CreatedAt = b.CreatedAt
        });
    }

    public async Task<IEnumerable<BookingOccasionDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingOccasionDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }
}