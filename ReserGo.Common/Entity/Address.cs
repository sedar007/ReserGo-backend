namespace ReserGo.Common.Entity;

public class Address {
    public int Id { get; set; }
    public String? Street { get; set; }
    public String? City { get; set; } 
    public String? State { get; set; }
    public String? PostalCode { get; set; } = null!;
    public String? Country { get; set; } = null!;
    
    // Relation One-to-One avec User
    public virtual User User { get; set; } = null!;
}