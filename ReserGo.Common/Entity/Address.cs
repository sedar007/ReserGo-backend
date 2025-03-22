namespace ReserGo.Common.Entity;

public class Address {
    public int Id { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; } 
    public string? State { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    
    // Relation One-to-One avec User
    public virtual User User { get; set; } = null!;
}