namespace Common.DTO;
public class OccasionDTO {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } 
    public int Capacity { get; set; } 
}