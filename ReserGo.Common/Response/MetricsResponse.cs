namespace ReserGo.Common.Response;

public class MetricsResponse {
    public int StatsNumber { get; set; }
    public Double? StatsPercent { get; set; }
    public Boolean? Up { get; set; } = null!;
}

