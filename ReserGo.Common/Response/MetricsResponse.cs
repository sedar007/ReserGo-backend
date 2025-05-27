namespace ReserGo.Common.Response;

public class MetricsResponse {
    public int StatsNumber { get; set; }
    public double? StatsPercent { get; set; }
    public bool? Up { get; set; } = null!;
}