using HavneApi.Data;
using HavneApi.Features.Berths;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Havne")));

var app = builder.Build();

app.MapBerthEndpoints();

app.MapGet("/", () => "HavneAPI is running ⚓");

app.Run();