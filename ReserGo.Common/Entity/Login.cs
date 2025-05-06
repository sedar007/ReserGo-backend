namespace ReserGo.Common.Entity;

public class Login {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public DateTime LastLogin { get; set; } = DateTime.Now;
    public int FailedAttempts { get; set; }
    public bool IsLocked { get; set; }

    // Relation avec User
    public virtual User User { get; init; } = null!;
    public Guid UserId { get; init; }
    
}