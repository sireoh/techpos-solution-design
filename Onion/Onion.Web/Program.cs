using Onion.Application.Services;
using Onion.Infrastructure.Data;
using Onion.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Onion.Domain.Interfaces;
using Onion.Web.Middleware; // important to import this as early as possible.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dependency Injection for Application Services and Repositories
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Dependency Injection for Application Services and Repositories
/*
AddTransient: Instant, short-lived instances. Instantiated fresh every single time they are called. Ideal for lightweight, operations-free utility elements.
AddScoped: Instance lifecycle tied strictly to a unique HTTP request pipeline context. Mandatory configuration pattern for EF Core DbContext implementations.
AddSingleton: Global singleton instantiation built once on root initialization and kept shared globally across all system threads. Good for static configurations or application-wide caching modules. 
*/
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();

var app = builder.Build();

// register the middleware in the pipeline
app.UseRequestPerformance();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
