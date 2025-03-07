namespace ReserGo.Common.DTO;

public class LoginDto {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
   
    public DateTime? LastLogin { get; set; }
    // Relation avec User
    public virtual UserDto User { get; set; } = null!;
    
    //[JsonIgnore]
    public string Password { get; set; } = null!;
    
}