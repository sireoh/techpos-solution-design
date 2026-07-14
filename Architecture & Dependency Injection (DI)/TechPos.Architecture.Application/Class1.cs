using Microsoft.Extensions.DependencyInjection;
using TechPos.Architecture.Domain;

namespace TechPos.Architecture.Application;

public sealed record CreateOrderRequest(string OrderNumber, string CustomerName, decimal TotalAmount);

public sealed record OrderSummaryDto(Guid Id, string OrderNumber, string CustomerName, decimal TotalAmount, string Status);

public interface IClock
{
    DateTime UtcNow { get; }
}

public interface IOrderAppService
{
    Task<IReadOnlyList<OrderSummaryDto>> GetOrdersAsync(CancellationToken cancellationToken = default);

    Task<OrderSummaryDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
}

public sealed class OrderAppService : IOrderAppService
{
    private readonly IOrderRepository _orderRepository;

    public OrderAppService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<OrderSummaryDto>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(Map).ToArray();
    }

    public async Task<OrderSummaryDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order(Guid.NewGuid(), request.OrderNumber, request.CustomerName, request.TotalAmount, OrderStatus.Pending);
        await _orderRepository.AddAsync(order, cancellationToken);
        return Map(order);
    }

    private static OrderSummaryDto Map(Order order)
    {
        return new OrderSummaryDto(order.Id, order.OrderNumber, order.CustomerName, order.TotalAmount, order.Status.ToString());
    }
}

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IOrderAppService, OrderAppService>();
        return services;
    }
}
