using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Entity;
using ReserGo.Common.Helper;
using ReserGo.Common.Requests.Notification;
using ReserGo.Common.Requests.Products.Hotel;
using ReserGo.Common.Response;
using ReserGo.Common.Security;
using ReserGo.DataAccess.Interfaces;
using ReserGo.Shared;

namespace ReserGo.Business.Implementations;

public class BookingHotelService : IBookingHotelService {
    private readonly IBookingHotelDataAccess _bookingHotelDataAccess;
    private readonly ILogger<BookingHotelService> _logger;
    private readonly INotificationService _notificationService;
    private readonly IRoomAvailabilityService _roomAvailabilityService;

    public BookingHotelService(ILogger<BookingHotelService> logger,
        IBookingHotelDataAccess bookingHotelDataAccess, INotificationService notificationService,
        IRoomAvailabilityService roomAvailabilityService) {
        _logger = logger;
        _bookingHotelDataAccess = bookingHotelDataAccess;
        _notificationService = notificationService;
        _roomAvailabilityService = roomAvailabilityService;
    }

    public async Task<BookingHotelResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user) {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var reservations = new List<BookingHotel>();
            foreach (var room in request.Rooms) {
                var availability =
                    await _roomAvailabilityService.GetAvailabilityByRoomId(room.RoomId, request.StartDate,
                        request.EndDate);

                if (availability == null || availability.StartDate > request.StartDate ||
                    availability.EndDate < request.EndDate ||
                    (await _bookingHotelDataAccess.GetBookingsByRoomId(room.RoomId))
                    .Any(b => request.StartDate < b.EndDate && request.EndDate > b.StartDate))
                    throw new InvalidDataException(
                        $"The room with ID {room.RoomId} is not available for the selected dates.");

                var totalDays = (request.EndDate.ToDateTime(TimeOnly.MinValue) -
                                 request.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
                var price = room.NumberOfGuests * (double)availability.Room.PricePerNight * totalDays;

                var reservation = new BookingHotel {
                    RoomId = room.RoomId,
                    HotelId = availability.Hotel.Id,
                    UserId = user.UserId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    NumberOfGuests = room.NumberOfGuests,
                    PriceTotal = price,
                    PricePerPerson = (double)availability.Room.PricePerNight,
                    IsConfirmed = true,
                    RoomAvailabilityId = availability.Id,
                    BookingDate = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                var createdReservation = await _bookingHotelDataAccess.Create(reservation);
                if (createdReservation == null) {
                    _logger.LogError("Booking hotel not created for RoomId: {RoomId}", room.RoomId);
                    throw new InvalidDataException($"Booking hotel not created for RoomId: {room.RoomId}");
                }

                reservations.Add(createdReservation);
            }

            var notification = new NotificationCreationRequest {
                Title = "New Reservation",
                Message = $"New reservation made by {user.Username} for {reservations.Count} rooms.",
                Type = "Hotel",
                Name = reservations.First().Hotel.Name,
                UserId = reservations.First().Hotel.UserId
            };
            var notificationDto = await _notificationService.CreateNotification(notification);

            return new BookingHotelResponses {
                Notification = notificationDto,
                Bookings = reservations.Select(r => r.ToDto())
            };
        
    }

    public async Task<IEnumerable<BookingHotelDto>> GetBookingsByUserId(Guid userId) {
        var bookings = await _bookingHotelDataAccess.GetBookingsByUserId(userId);
        return bookings.Select(b => new BookingHotelDto {
            Id = b.Id,
            RoomId = b.RoomId,
            HotelId = b.HotelId,
            UserId = b.UserId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            NumberOfGuests = b.NumberOfGuests,
            IsConfirmed = b.IsConfirmed,
            BookingDate = b.BookingDate
        });
    }

    public async Task<IEnumerable<BookingHotelDto>> GetBookingsByAdminId(Guid adminId) {
        var bookings = await _bookingHotelDataAccess.GetBookingsByAdminId(adminId);
        return bookings.Select(b => b.ToDto());
    }
}