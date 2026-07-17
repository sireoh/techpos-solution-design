using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onion.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Maps to the "Orders" table in SQL Server
            builder.ToTable("Orders");

            // Primary Key configuration
            builder.HasKey(o => o.Id);

            // Property constraints
            builder.Property(o => o.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
