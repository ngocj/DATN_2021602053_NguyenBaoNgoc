using Microsoft.EntityFrameworkCore;
using SP.Domain.Entity;
using SP.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Context
{
    public class SPContext : DbContext
    {
        public SPContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }      
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FeedBack> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Image> Images { get; set; }


        public DbSet<Ward> Wards { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new WardConfiguration());
            modelBuilder.ApplyConfiguration(new DistrictConfiguration());
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
            modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, RoleName = "Admin" },
               new Role { Id = 2, RoleName = "Manager" },
               new Role { Id = 3, RoleName = "Employee" },
               new Role { Id = 4, RoleName = "User" }
           );
            
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "admin", Email = "admin@gmail.com", Password = "admin", RoleId = 1 },
                new User { Id = 2, UserName = "Nguyen Bao Ngoc", Email = "ngoc@gmail.com", Password = "ngoc123" }

            );
            // brand
            var defaultDate = new DateTime(2024, 01, 01);

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, BrandName = "Nike", Description = "Thương hiệu thể thao nổi tiếng", CreatedAt = defaultDate },
                new Brand { Id = 2, BrandName = "Adidas", Description = "Thương hiệu đến từ Đức", CreatedAt = defaultDate },
                new Brand { Id = 3, BrandName = "Puma", Description = "Nổi bật với thiết kế năng động", CreatedAt = defaultDate },
                new Brand { Id = 4, BrandName = "Under Armour", Description = "Chuyên đồ thể thao hiệu suất cao", CreatedAt = defaultDate },
                new Brand { Id = 5, BrandName = "New Balance", Description = "Nổi bật với giày chạy bộ", CreatedAt = defaultDate }
            );
            // category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, CategoryName = "Giày", Description = "Các loại giày thể thao", CreatedAt = defaultDate },
                new Category { Id = 2, CategoryName = "Áo", Description = "Áo thể thao nam nữ", CreatedAt = defaultDate },
                new Category { Id = 3, CategoryName = "Quần", Description = "Quần thể thao, quần short", CreatedAt = defaultDate },
                new Category { Id = 4, CategoryName = "Phụ kiện", Description = "Túi, nón, tất thể thao", CreatedAt = defaultDate },
                new Category { Id = 5, CategoryName = "Đồ tập gym", Description = "Trang phục và dụng cụ tập gym", CreatedAt = defaultDate }
);


            // add subcategory
            modelBuilder.Entity<SubCategory>().HasData(
                new SubCategory { Id = 1, Name = "Giày thể thao", CategoryId = 1 },
                new SubCategory { Id = 2, Name = "Giày cao gót", CategoryId = 1 },
                new SubCategory { Id = 3, Name = "Áo thun", CategoryId = 2 },
                new SubCategory { Id = 4, Name = "Áo khoác", CategoryId = 2 },
                new SubCategory { Id = 5, Name = "Quần jean", CategoryId = 3 },
                new SubCategory { Id = 6, Name = "Quần short", CategoryId = 3 }
            );

            // discsount
            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    Percent = 10,
                    Description = "Giảm 10% mừng khai trương",
                    IsActive = true,
                    DateStart = defaultDate,
                    DateEnd = defaultDate.AddMonths(1)
                },
                new Discount
                {
                    Id = 2,
                    Percent = 15,
                    Description = "Ưu đãi mùa hè",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(5),
                    DateEnd = defaultDate.AddMonths(2)
                },
                new Discount
                {
                    Id = 3,
                    Percent = 20,
                    Description = "Giảm giá sốc cuối tuần",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(10),
                    DateEnd = defaultDate.AddMonths(1).AddDays(10)
                },
                new Discount
                {
                    Id = 4,
                    Percent = 5,
                    Description = "Giảm nhẹ cho khách quen",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(3),
                    DateEnd = defaultDate.AddMonths(3)
                },
                new Discount
                {
                    Id = 5,
                    Percent = 25,
                    Description = "Black Friday Sale",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(1),
                    DateEnd = defaultDate.AddMonths(1).AddDays(5)
                }
            );
        }


    }
}
