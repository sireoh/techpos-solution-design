using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Initializes a new instance of the Order class with
        /// the specified customer name and total amount.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="totalAmount"></param>
        public Order(string customerName, decimal totalAmount) { 
            Id = Guid.NewGuid();
            CustomerName = customerName;
            TotalAmount = totalAmount;
        }
    }
}
