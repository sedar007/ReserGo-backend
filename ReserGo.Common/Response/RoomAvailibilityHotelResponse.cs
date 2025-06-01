using ReserGo.Common.DTO;
namespace ReserGo.Common.Response;

public class RoomAvailibilityHotelResponse {
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string ImageSrc { get; set; } = string.Empty;
    public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}