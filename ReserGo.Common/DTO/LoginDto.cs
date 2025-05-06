namespace ReserGo.Common.DTO;

public class LoginDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    // Relation avec User
    public virtual UserDto User { get; set; } = null!;

    //[JsonIgnore]
    public string Password { get; set; } = null!;
}