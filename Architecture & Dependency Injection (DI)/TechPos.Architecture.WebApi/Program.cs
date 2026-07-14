using TechPos.Architecture.Application;
using TechPos.Architecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/diagnostics/time", (IClock clock) => Results.Ok(new { clock.UtcNow }));

var orders = app.MapGroup("/orders");

orders.MapGet("/", async (IOrderAppService service, CancellationToken cancellationToken) =>
{
    var result = await service.GetOrdersAsync(cancellationToken);
    return Results.Ok(result);
});

orders.MapPost("/", async (CreateOrderRequest request, IOrderAppService service, CancellationToken cancellationToken) =>
{
    var created = await service.CreateOrderAsync(request, cancellationToken);
    return Results.Created($"/orders/{created.Id}", created);
});

app.Run();
