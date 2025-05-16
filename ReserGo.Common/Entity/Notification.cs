namespace ReserGo.Common.Entity;

public class Notification {
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}