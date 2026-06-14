using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Business Rule: 
            builder.HasIndex(c => c.Email)
                   .IsUnique();

            // Business Rule:
            builder.Property(c => c.PhoneNumber)
                   .IsRequired();
        }
    }
}