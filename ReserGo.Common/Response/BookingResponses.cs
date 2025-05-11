using ReserGo.Common.Enum;
using ReserGo.Common.DTO;
namespace ReserGo.Common.Response;

public class BookingResponses {
   public BookingHotelDto BookingHotel { get; set; }
   public NotificationDto Notification { get; set; }
}