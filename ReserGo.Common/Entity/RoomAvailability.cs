namespace ReserGo.Common.Entity;

public class RoomAvailability {
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public virtual Room Room { get; init; } = null!;
    public virtual Hotel Hotel { get; init; } = null!;
    
    public ICollection<BookingHotel> BookingsHotels { get; init; } = new List<BookingHotel>();

}