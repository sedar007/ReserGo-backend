namespace ReserGo.Common.DTO;
public class AddressDto {
    public int Id { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; } 
    public string? State { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    
}