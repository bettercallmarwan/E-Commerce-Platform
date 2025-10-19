using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Products
{
    internal class ProductConfigurations : BaseAuditableEntityConfigurations<Product, int>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(E => E.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(E => E.NormalizedName)
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Property(E => E.Description)
                .IsRequired();

            builder.Property(E => E.Price)
                .HasColumnType("decimal(9, 2)");

            builder.HasOne(E => E.Brand)
                .WithMany()
                .HasForeignKey(E => E.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(E => E.Category)
                .WithMany()
                .HasForeignKey(E => E.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
