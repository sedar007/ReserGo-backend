namespace ReserGo.Common.DTO;

public abstract class BookingDto {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly BookingDate { get; set; }
    public double PriceTotal { get; set; }

    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
}