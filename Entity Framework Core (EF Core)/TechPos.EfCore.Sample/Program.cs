using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechPos.EfCore.Sample;

using var connection = new SqliteConnection("Data Source=:memory:");
await connection.OpenAsync();

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connection)
    .Options;

using var context = new AppDbContext(options);
await context.Database.EnsureCreatedAsync();

if (!await context.Customers.AnyAsync())
{
    await SeedAsync(context);
}

var orderDetails = await (
    from order in context.Orders
    join customer in context.Customers on order.CustomerId equals customer.Id
    where order.Status == OrderStatus.Pending
    select new
    {
        order.Id,
        customer.Name,
        order.TotalAmount
    }).ToListAsync();

Console.WriteLine("Pending orders joined to customers:");
foreach (var detail in orderDetails)
{
    Console.WriteLine($"{detail.Id} | {detail.Name} | {detail.TotalAmount:C}");
}

var regionalTotals = await context.Orders
    .GroupBy(order => order.Region)
    .Select(group => new
    {
        Region = group.Key,
        TotalOrders = group.Count(),
        TotalRevenue = group.Sum(order => order.TotalAmount),
        AverageOrderValue = group.Average(order => order.TotalAmount)
    })
    .Where(summary => summary.TotalRevenue >= 500)
    .OrderByDescending(summary => summary.TotalRevenue)
    .ToListAsync();

Console.WriteLine();
Console.WriteLine("Regional revenue summary:");
foreach (var summary in regionalTotals)
{
    Console.WriteLine($"{summary.Region} | {summary.TotalOrders} | {summary.TotalRevenue:C} | {summary.AverageOrderValue:C}");
}

var averageOrderAmount = await context.Orders.AverageAsync(order => order.TotalAmount);
var technologyCustomers = await context.Customers
    .Where(customer =>
        customer.Industry == "Technology" &&
        context.Orders.Any(order => order.CustomerId == customer.Id && order.TotalAmount > averageOrderAmount))
    .Select(customer => new { customer.Id, customer.Name })
    .ToListAsync();

Console.WriteLine();
Console.WriteLine("Technology customers above average order value:");
foreach (var customer in technologyCustomers)
{
    Console.WriteLine($"{customer.Id} | {customer.Name}");
}

await context.Orders
    .Where(order => order.Status == OrderStatus.Pending)
    .ExecuteUpdateAsync(setters => setters.SetProperty(order => order.Status, OrderStatus.Shipped));

var deletedCount = await context.Orders
    .Where(order => order.TotalAmount < 60)
    .ExecuteDeleteAsync();

Console.WriteLine();
Console.WriteLine("Completed update and delete operations.");
Console.WriteLine($"Deleted low-value orders: {deletedCount}");

static async Task SeedAsync(AppDbContext context)
{
    var customers = new[]
    {
        new Customer
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "Contoso",
            Industry = "Technology"
        },
        new Customer
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "Northwind",
            Industry = "Retail"
        },
        new Customer
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Name = "Fabrikam",
            Industry = "Technology"
        }
    };

    var orders = new[]
    {
        new Order
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CustomerId = customers[0].Id,
            OrderDate = new DateOnly(2026, 07, 01),
            TotalAmount = 125.50m,
            Region = "North",
            Status = OrderStatus.Pending
        },
        new Order
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            CustomerId = customers[1].Id,
            OrderDate = new DateOnly(2026, 07, 02),
            TotalAmount = 349.99m,
            Region = "North",
            Status = OrderStatus.Shipped
        },
        new Order
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            CustomerId = customers[2].Id,
            OrderDate = new DateOnly(2026, 07, 03),
            TotalAmount = 780.00m,
            Region = "West",
            Status = OrderStatus.Pending
        },
        new Order
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            CustomerId = customers[2].Id,
            OrderDate = new DateOnly(2026, 07, 04),
            TotalAmount = 45.00m,
            Region = "West",
            Status = OrderStatus.Pending
        }
    };

    context.Customers.AddRange(customers);
    context.Orders.AddRange(orders);
    await context.SaveChangesAsync();
}
