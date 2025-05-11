namespace ReserGo.Common.Requests.Notification;

public class NotificationCreationRequest {
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string HotelName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public Guid UserId { get; init; }
}