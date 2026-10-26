namespace HavneApi.Features.Berths;

public class Berth
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double MaxLengthMeters { get; set; }
    public double MaxDepthMeters { get; set; }
}