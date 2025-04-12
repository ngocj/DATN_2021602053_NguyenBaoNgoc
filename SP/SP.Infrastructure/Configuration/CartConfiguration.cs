using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder.Property(c => c.Quantity).IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.UpdatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.ProductVariant)
                .WithMany(pv => pv.Carts)
                .HasForeignKey(c => c.ProductVariantId);
        }
    }   
}
