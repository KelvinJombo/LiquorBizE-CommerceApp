﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odering.Domain.Models;
using Odering.Domain.ValueObjects;

namespace Odering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,
                dbId => CustomerId.Of(dbId));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255);
            builder.Property(c => c.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }

     
}
