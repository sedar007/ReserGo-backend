using Microsoft.Extensions.Logging;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
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
    private readonly IRoomAvailabilityService _roomAvailabilityService;

    public BookingHotelService(ILogger<BookingHotelService> logger,
        IHotelOfferService hotelOfferService,
        IBookingHotelDataAccess bookingHotelDataAccess, INotificationService notificationService,
        IRoomAvailabilityService roomAvailabilityService) {
        _logger = logger;
        _hotelOfferService = hotelOfferService;
        _bookingHotelDataAccess = bookingHotelDataAccess;
        _notificationService = notificationService;
        _roomAvailabilityService = roomAvailabilityService;
    }

    public async Task<BookingResponses> CreateBooking(BookingHotelRequest request, ConnectedUser user) {
        try {
            if (user == null) {
                _logger.LogError("User not found");
                throw new InvalidDataException(Consts.UserNotFound);
            }

            var availability = await _roomAvailabilityService.GetAvailabilityByRoomId(request.RoomId);

            if (availability == null || availability.StartDate.Date > request.StartDate.Date ||
                availability.EndDate.Date < request.EndDate.Date)
                throw new InvalidDataException("The room is not available for the selected dates.");

            var existingBookings = await _bookingHotelDataAccess.GetBookingsByRoomId(request.RoomId);
            if (existingBookings.Any(b =>
                    request.StartDate.Date < b.EndDate.Date && request.EndDate.Date > b.StartDate.Date))
                throw new InvalidDataException("The room is already booked for the selected dates.");

            var price = request.NumberOfGuests * (double)availability.Room.PricePerNight * 
                        (request.EndDate.Date - request.StartDate.Date).TotalDays;
            var reservation = new BookingHotel {
                RoomId = request.RoomId,
                HotelId = availability.Hotel.Id,
                UserId = request.UserId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                NumberOfGuests = request.NumberOfGuests,
                Price = price,
                IsConfirmed = request.IsConfirmed,
                RoomAvailabilityId = availability.Id,
                BookingDate = DateTime.UtcNow
            };

            var createdReservation = await _bookingHotelDataAccess.Create(reservation);
            if (createdReservation == null) {
                _logger.LogError("Booking hotel not created");
                throw new InvalidDataException("Booking hotel not created");
            }

            var bookingHotel = createdReservation?.ToDto();

            var notification = new NotificationCreationRequest {
                Title = "New Reservation",
                Message = $"New reservation made by {user.Username} for offer at {availability.Hotel.Name} " +
                          $"number of guests: {request.NumberOfGuests}",
                Type = "Hotel",
                Name = availability.Hotel.Name,
                UserId = availability.Hotel.UserId
            };
            var notificationDto = await _notificationService.CreateNotification(notification);

            return new BookingResponses {
                Notification = notificationDto,
                Booking = bookingHotel
            };
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
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