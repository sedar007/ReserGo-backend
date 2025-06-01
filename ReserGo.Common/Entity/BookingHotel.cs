namespace ReserGo.Common.Entity;

public class BookingHotel : Booking {
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public double PricePerPerson { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public virtual Room Room { get; set; } = null!;
    public virtual Hotel Hotel { get; set; } = null!;

    public Guid RoomAvailabilityId { get; set; }
    public virtual RoomAvailability RoomAvailability { get; set; } = null!;
}