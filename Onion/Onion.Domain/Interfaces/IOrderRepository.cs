using Onion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id);

        Task AddAsync(Order order);

        Task<IEnumerable<dynamic>> GetPendingOrderDetailsAsync();

        Task<bool> UpdateOrderStatusToShippedAsync(Guid orderId);

        Task<bool> DeleteOrderDirectAsync(Guid orderId);
    }
}
