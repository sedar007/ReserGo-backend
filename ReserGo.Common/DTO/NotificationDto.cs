namespace ReserGo.Common.DTO;

public class NotificationDto {
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string HotelName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public Guid UserId { get; init; }
    public UserDto User { get; init; } = null!;
}