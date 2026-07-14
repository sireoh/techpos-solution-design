using TechPos.MiddlewarePipeline.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler("/error");
app.UseRequestPerformance();

app.MapGet("/api/ping", () => Results.Ok(new { Message = "pong" }));
app.MapGet("/error", () => Results.Problem("An unexpected error occurred."));

app.Run();
