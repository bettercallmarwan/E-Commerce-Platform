using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence._Data.Config.Orders
{
    internal class OrderConfigurations : BaseAuditableEntityConfigurations<Order, int>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(order => order.ShippingAddress);

            // enum mapping
            builder.Property(order => order.Status)
                .HasConversion
                (
                    (OStatus) => OStatus.ToString(), // from enum to string (for storing in db)
                    (OStatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus) // from string to enum (retrieving from db)
                );

            builder.Property(order => order.SubTotal)
                .HasColumnType("decimal(8, 2)");

            builder.HasOne(order => order.DeliveryMethod)
                .WithMany()
                .HasForeignKey(order => order.DeliveryMethodId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(order => order.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
