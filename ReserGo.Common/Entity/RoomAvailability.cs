namespace ReserGo.Common.Entity;

public class RoomAvailability {
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public virtual Room Room { get; set; } = null!;
    public virtual Hotel Hotel { get; set; } = null!;
    
    public ICollection<BookingHotel> BookingsHotels { get; set; } = new List<BookingHotel>();

}