namespace ReserGo.Common.Entity;

public class Address {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    // Relation One-to-One avec User
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}