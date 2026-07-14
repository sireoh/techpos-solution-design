namespace TechPos.Architecture.Domain;

public enum OrderStatus
{
    Pending,
    Shipped,
    Cancelled
}

public sealed class Order
{
    public Order(Guid id, string orderNumber, string customerName, decimal totalAmount, OrderStatus status)
    {
        Id = id;
        OrderNumber = orderNumber;
        CustomerName = customerName;
        TotalAmount = totalAmount;
        Status = status;
    }

    public Guid Id { get; }

    public string OrderNumber { get; }

    public string CustomerName { get; }

    public decimal TotalAmount { get; private set; }

    public OrderStatus Status { get; private set; }

    public void MarkShipped() => Status = OrderStatus.Shipped;
}

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Order order, CancellationToken cancellationToken = default);
}
