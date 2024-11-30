﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odering.Domain.Enums;
using Odering.Domain.Models;
using Odering.Domain.ValueObjects;

namespace Odering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasConversion(
                orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            builder.ComplexProperty(
                o => o.OrderName, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
                });


            builder.ComplexProperty(
                o => o.DeliveryAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();


                    addressBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                    addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                    addressBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();


                    addressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);


                    addressBuilder.Property(a => a.State)
                    .HasMaxLength(50)
                    .IsRequired();

                    addressBuilder.Property(a => a.Town)
                    .HasMaxLength(50)
                    .IsRequired();


                });
                
                builder.ComplexProperty(
                o => o.CompanyAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                    addressBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                    addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);

                    addressBuilder.Property(a => a.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();


                    addressBuilder.Property(a => a.Country)
                    .HasMaxLength(50);


                    addressBuilder.Property(a => a.State)
                    .HasMaxLength(50)
                    .IsRequired();

                    addressBuilder.Property(a => a.Town)
                    .HasMaxLength(50)
                    .IsRequired();


                    builder.ComplexProperty(
                        o => o.Payment, paymentBuilder =>
                        {
                            paymentBuilder.Property(p => p.CardName)
                            .HasMaxLength(50);

                            paymentBuilder.Property(p => p.CardNumber)
                            .HasMaxLength(25)
                            .IsRequired();


                            paymentBuilder.Property(p => p.ExpiryDate)
                            .HasMaxLength(10)
                            .IsRequired();


                            paymentBuilder.Property(p => p.CVV)
                            .HasMaxLength(3);

                            paymentBuilder.Property(p => p.PaymentMethod);
                        });


                    builder.Property(o => o.Status)
                    .HasDefaultValue(OrderStatus.Draft)
                    .HasConversion(s => s.ToString(),
                    dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));


                    builder.Property(o => o.TotalPrice);

                });
        }
    }
}