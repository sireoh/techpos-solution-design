using Microsoft.Extensions.DependencyInjection;
using TechPos.Architecture.Application;
using TechPos.Architecture.Domain;

namespace TechPos.Architecture.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public sealed class OrderCatalog
{
    public List<Order> Orders { get; } =
    [
        new Order(Guid.Parse("11111111-1111-1111-1111-111111111111"), "ORD-1001", "Contoso", 125.50m, OrderStatus.Pending),
        new Order(Guid.Parse("22222222-2222-2222-2222-222222222222"), "ORD-1002", "Northwind", 349.99m, OrderStatus.Shipped)
    ];
}

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly OrderCatalog _catalog;

    public InMemoryOrderRepository(OrderCatalog catalog)
    {
        _catalog = catalog;
    }

    public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Order>>(_catalog.Orders.ToArray());
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = _catalog.Orders.FirstOrDefault(item => item.Id == id);
        return Task.FromResult(order);
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _catalog.Orders.Add(order);
        return Task.CompletedTask;
    }
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<OrderCatalog>();
        services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
        return services;
    }
}
