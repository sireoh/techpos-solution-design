namespace TechPos.EfCore.Sample;

public enum OrderStatus
{
    Pending,
    Shipped,
    Cancelled
}

public sealed class Customer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

public sealed class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Region { get; set; } = string.Empty;

    public OrderStatus Status { get; set; }
}