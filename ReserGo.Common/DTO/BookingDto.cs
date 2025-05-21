namespace ReserGo.Common.DTO;

public abstract class BookingDto {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}