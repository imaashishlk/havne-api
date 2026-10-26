namespace HavneApi.Features.Berths;

public record CreateBerthRequest(string Name, double MaxLengthMeters, double MaxDepthMeters);

public record BerthResponse(int Id, string Name, double MaxLengthMeters, double MaxDepthMeters);