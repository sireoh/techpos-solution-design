using Onion.Domain.Interfaces;
using Onion.Domain.Entities;
using Onion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Onion.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        { 
            _context = context;
        }

        public async Task<Order> GetByIdAsync(Guid id) => await _context.Orders.FindAsync(id);

        public async Task AddAsync(Order order)
        { 
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<dynamic>> GetPendingOrderDetailsAsync()
        {
            var orderDetails = await _context.Orders
                .Where(o => o.Status == "Pending")
                .Select(o => new
                {
                    OrderId = o.Id,
                    CustomerName = o.CustomerName,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return orderDetails.Cast<dynamic>();
        }

        public async Task<bool> UpdateOrderStatusToShippedAsync(Guid orderId)
        {
            var rowsAffected = await _context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(o => o.Status, "Shipped"));

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteOrderDirectAsync(Guid orderId)
        {
            var rowsAffected = await _context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }

    }
}
