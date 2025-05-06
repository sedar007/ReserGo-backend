namespace ReserGo.Common.Entity;

public class Login {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime LastLogin { get; set; } = DateTime.Now;
    public int FailedAttempts { get; set; } = 0;
    public bool IsLocked { get; set; } = false;

    // Relation avec User
    public virtual User User { get; set; } = null!;
}