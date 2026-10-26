using HavneApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HavneApi.Features.Berths;

public static class BerthEndpoints
{
    public static void MapBerthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/berths").WithTags("Berths");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:int}", GetById).WithName("GetBerthById");
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);
    }

    static async Task<Ok<List<BerthResponse>>> GetAll(AppDbContext db, CancellationToken ct)
    {
        var berths = await db.Berths
            .AsNoTracking()
            .Select(b => new BerthResponse(b.Id, b.Name, b.MaxLengthMeters, b.MaxDepthMeters))
            .ToListAsync(ct);

        return TypedResults.Ok(berths);
    }

    static async Task<Results<Ok<BerthResponse>, NotFound>> GetById(int id, AppDbContext db, CancellationToken ct)
    {
        var berth = await db.Berths
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BerthResponse(b.Id, b.Name, b.MaxLengthMeters, b.MaxDepthMeters))
            .FirstOrDefaultAsync(ct);

        return berth is null ? TypedResults.NotFound() : TypedResults.Ok(berth);
    }

    static async Task<CreatedAtRoute<BerthResponse>> Create(CreateBerthRequest request, AppDbContext db, CancellationToken ct)
    {
        var berth = new Berth
        {
            Name = request.Name,
            MaxLengthMeters = request.MaxLengthMeters,
            MaxDepthMeters = request.MaxDepthMeters
        };

        db.Berths.Add(berth);
        await db.SaveChangesAsync(ct);

        var response = new BerthResponse(berth.Id, berth.Name, berth.MaxLengthMeters, berth.MaxDepthMeters);
        return TypedResults.CreatedAtRoute(response, "GetBerthById", new { id = berth.Id });
    }

    static async Task<Results<NoContent, NotFound>> Update(int id, CreateBerthRequest request, AppDbContext db, CancellationToken ct)
    {
        var berth = await db.Berths.FindAsync([id], ct);
        if (berth is null) return TypedResults.NotFound();

        berth.Name = request.Name;
        berth.MaxLengthMeters = request.MaxLengthMeters;
        berth.MaxDepthMeters = request.MaxDepthMeters;

        await db.SaveChangesAsync(ct);
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> Delete(int id, AppDbContext db, CancellationToken ct)
    {
        var deleted = await db.Berths.Where(b => b.Id == id).ExecuteDeleteAsync(ct);
        return deleted > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}