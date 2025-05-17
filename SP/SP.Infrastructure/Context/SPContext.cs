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
               new User
               {
                   Id = new Guid("f75a0e6e-9f2d-4a39-9040-548b0e56e022"),
                   UserName = "admin",
                   Email = "admin@gmail.com",
                   Password = "admin",
                   RoleId = 1, // Admin
                   DateOfBirth = new DateOnly(1985, 4, 12),
                   PhoneNumber = "0909123456",
                   AddressDetail = "123 Đường Lê Lợi, Quận 1, TP. Hồ Chí Minh"
               },
              new User
              {
                  Id = new Guid("c0a4fd1a-d56b-4bc1-8925-f29be9a383be"),
                  UserName = "manager",
                  Email = "manager@gmail.com",
                  Password = "manager",
                  RoleId = 2, // Manager
                  DateOfBirth = new DateOnly(1980, 9, 25),
                  PhoneNumber = "0909999999",
                  AddressDetail = "100 Đường Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh"
              },
              new User
              {
                  Id = new Guid("a1f3e4c5-b67d-4d9f-8c2b-33b7f7f4a7c1"),
                  UserName = "nguyenvana",
                  Email = "nguyenvana@gmail.com",
                  Password = "user123",
                  RoleId = 4, // User
                  DateOfBirth = new DateOnly(1995, 5, 10),
                  PhoneNumber = "0911222333",
                  AddressDetail = "456 Phố Huế, Quận Hai Bà Trưng, Hà Nội"
              },
              new User
              {
                  Id = new Guid("b2d4c6e7-f89a-45bc-a123-4d56e78f9012"),
                  UserName = "tranthib",
                  Email = "tranthib@gmail.com",
                  Password = "user456",
                  RoleId = 4,
                  DateOfBirth = new DateOnly(1997, 3, 15),
                  PhoneNumber = "0933444555",
                  AddressDetail = "789 Đường Trần Phú, Thành phố Đà Nẵng"
              },
              new User
              {
                  Id = new Guid("c3e5d7f8-a12b-46cd-b234-5e67f89a0123"),
                  UserName = "lethic",
                  Email = "lethic@gmail.com",
                  Password = "user789",
                  RoleId = 4,
                  DateOfBirth = new DateOnly(1996, 8, 22),
                  PhoneNumber = "0909777888",
                  AddressDetail = "11 Nguyễn Trãi, Quận Thanh Xuân, Hà Nội"
              }
            );
            modelBuilder.Entity<Employee>().HasData(
           new Employee
           {
               Id = new Guid("d4f6e8a9-b23c-47de-c345-6f78a90b1234"),
               Name = "anhkhoa",
               Email = "anhkhoa@gmail.com",
               Password = "anhkhoa",
               RoleId = 3,
               DateOfBirth = new DateOnly(1992, 6, 20),
               PhoneNumber = "0909666777",
               AddressDetail = "123 Lê Lợi, Quận 1, TP. Hồ Chí Minh"
           },
           new Employee
           {
               Id = new Guid("e5f7a9b0-c34d-58ef-d456-7f89b01c2345"),
               Name = "thuyngan",
               Email = "thuyngan@gmail.com",
               Password = "thuyngan",
               RoleId = 3,
               DateOfBirth = new DateOnly(1993, 8, 5),
               PhoneNumber = "0909777888",
               AddressDetail = "456 Nguyễn Huệ, Quận 3, TP. Hồ Chí Minh"
           },
           new Employee
           {
               Id = new Guid("f6a8b0c1-d45e-69ef-e567-8a90b12d3456"),
               Name = "vandung",
               Email = "vandung@gmail.com",
               Password = "vandung",
               RoleId = 3,
               DateOfBirth = new DateOnly(1994, 12, 10),
               PhoneNumber = "0909888999",
               AddressDetail = "789 Phan Đình Phùng, Quận Bình Thạnh, TP. Hồ Chí Minh"
           }
       );
            // 5 products
            var defaultDateProduct = new DateTime(2024, 01, 01);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    ProductName = "Giày Nike Air Max",
                    Description = "Giày thể thao Nike Air Max chính hãng",
                    SubCategoryId = 1,
                    BrandId = 1,
                    CreatedAt = defaultDateProduct
                },
                new Product
                {
                    Id = 2,
                    ProductName = "Áo Adidas Originals",
                    Description = "Áo thun Adidas Originals nam nữ",
                    SubCategoryId = 3,
                    BrandId = 2,
                    CreatedAt = defaultDateProduct
                },
                new Product
                {
                    Id = 3,
                    ProductName = "Quần Puma Sport",
                    Description = "Quần thể thao Puma Sport",
                    SubCategoryId = 5,
                    BrandId = 3,
                    CreatedAt = defaultDateProduct
                },
                new Product
                {
                    Id = 4,
                    ProductName = "Giày New Balance Fresh Foam",
                    Description = "Giày chạy bộ New Balance Fresh Foam",
                    SubCategoryId = 1,
                    BrandId = 5,
                    CreatedAt = defaultDateProduct
                },
                new Product
                {
                    Id = 5,
                    ProductName = "Áo khoác Under Armour",
                    Description = "Áo khoác thể thao Under Armour",
                    SubCategoryId = 4,
                    BrandId = 4,
                    CreatedAt = defaultDateProduct
                }
            );
            // 5 product variants
            modelBuilder.Entity<ProductVariant>().HasData(
                new ProductVariant
                {
                    Id = 1,
                    ProductId = 1,
                    Size = "M",
                    Color = "black",
                    Price = 1500000,
                    Quantity = 100,
                    CreatedAt = defaultDateProduct
                },
                new ProductVariant
                {
                    Id = 2,
                    ProductId = 1,
                    Size = "L",
                    Color = "white",
                    Price = 1600000,
                    Quantity = 50,
                    CreatedAt = defaultDateProduct
                },
                new ProductVariant
                {
                    Id = 3,
                    ProductId = 2,
                    Size = "S",
                    Color = "green",
                    Price = 800000,
                    Quantity = 200,
                    CreatedAt = defaultDateProduct
                },
                new ProductVariant
                {
                    Id = 4,
                    ProductId = 3,
                    Size = "M",
                    Color = "red",
                    Price = 1200000,
                    Quantity = 80,
                    CreatedAt = defaultDateProduct
                },
                new ProductVariant
                {
                    Id = 5,
                    ProductId = 4,
                    Size = "L",
                    Color = "yellow",
                    Price = 1700000,
                    Quantity = 30,
                    CreatedAt = defaultDateProduct
                }
            );
            // 5 orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c193"),
                    UserId = new Guid("f75a0e6e-9f2d-4a39-9040-548b0e56e022"),
                    EmployeeId = new Guid("d4f6e8a9-b23c-47de-c345-6f78a90b1234"),
                    Status = OrderStatus.Pending,
                    TotalPrice = 3000000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new Order
                {
                    Id = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c111"),
                    UserId = new Guid("c0a4fd1a-d56b-4bc1-8925-f29be9a383be"),
                    EmployeeId = new Guid("e5f7a9b0-c34d-58ef-d456-7f89b01c2345"),
                    Status = OrderStatus.Confirmed,
                    TotalPrice = 1600000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new Order
                {
                    Id = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c666"),
                    UserId = new Guid("a1f3e4c5-b67d-4d9f-8c2b-33b7f7f4a7c1"),
                    EmployeeId = new Guid("f6a8b0c1-d45e-69ef-e567-8a90b12d3456"),
                    Status = OrderStatus.Shipping,
                    TotalPrice = 2400000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new Order
                {
                    Id = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c178"),
                    UserId = new Guid("b2d4c6e7-f89a-45bc-a123-4d56e78f9012"),
                    EmployeeId = new Guid("d4f6e8a9-b23c-47de-c345-6f78a90b1234"),
                    Status = OrderStatus.Delivered,
                    TotalPrice = 2400000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new Order
                {
                    Id = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c192"),
                    UserId = new Guid("c3e5d7f8-a12b-46cd-b234-5e67f89a0123"),
                    EmployeeId = new Guid("e5f7a9b0-c34d-58ef-d456-7f89b01c2345"),
                    Status = OrderStatus.Canceled,
                    TotalPrice = 1700000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                }
            );
            
            // 5 order details
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c193"),
                    ProductVariantId = 1,
                    Quantity = 2,
                    Price = 1500000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new OrderDetail
                {
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c111"),
                    ProductVariantId = 2,
                    Quantity = 1,
                    Price = 1600000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new OrderDetail
                {
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c666"),
                    ProductVariantId = 3,
                    Quantity = 3,
                    Price = 800000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new OrderDetail
                {
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c178"),
                    ProductVariantId = 4,
                    Quantity = 2,
                    Price = 1200000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                },
                new OrderDetail
                {
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c192"),
                    ProductVariantId = 5,
                    Quantity = 1,
                    Price = 1700000,
                    CreatedAt = defaultDateProduct,
                    UpdatedAt = defaultDateProduct
                }
            );
            // 5 carts
            modelBuilder.Entity<Cart>().HasData(
                new Cart
                {                   
                    UserId = new Guid("f75a0e6e-9f2d-4a39-9040-548b0e56e022"),
                    ProductVariantId = 1,
                    Quantity = 1
                },
                new Cart
                {

                    UserId = new Guid("c0a4fd1a-d56b-4bc1-8925-f29be9a383be"),
                    ProductVariantId = 2,
                    Quantity = 2
                },
                new Cart
                {
                    UserId = new Guid("a1f3e4c5-b67d-4d9f-8c2b-33b7f7f4a7c1"),
                    ProductVariantId = 3,
                    Quantity = 3
                },
                new Cart
                {
                    UserId = new Guid("b2d4c6e7-f89a-45bc-a123-4d56e78f9012"),
                    ProductVariantId = 4,
                    Quantity = 1
                },
                new Cart
                {
                    UserId = new Guid("c3e5d7f8-a12b-46cd-b234-5e67f89a0123"),
                    ProductVariantId = 5,
                    Quantity = 2
                }
            );
            // 5 feedbacks
            modelBuilder.Entity<FeedBack>().HasData(
                new FeedBack
                {
                    Id = 1, 
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c193"),
                    ProductVariantId = 1,
                    UserId = new Guid("f75a0e6e-9f2d-4a39-9040-548b0e56e022"),
                    Comment = "Sản phẩm rất tốt!",
                    Rating = 4.5
                },
                new FeedBack
                {
                    Id = 2,
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c111"),
                    ProductVariantId = 2,
                    UserId = new Guid("c0a4fd1a-d56b-4bc1-8925-f29be9a383be"),
                    Comment = "Chất lượng không như mong đợi.",
                    Rating = 2.5
                },
                new FeedBack
                {
                    Id = 3,
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c666"),
                    ProductVariantId = 3,
                    UserId = new Guid("a1f3e4c5-b67d-4d9f-8c2b-33b7f7f4a7c1"),
                    Comment = "Rất hài lòng với sản phẩm này.",
                    Rating = 5.0
                },
                new FeedBack
                {
                    Id = 4,
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c178"),
                    ProductVariantId = 4,
                    UserId = new Guid("b2d4c6e7-f89a-45bc-a123-4d56e78f9012"),
                    Comment = "Giao hàng nhanh chóng, sản phẩm chất lượng.",
                    Rating = 4.0
                },
                new FeedBack
                {
                    Id = 5,
                    OrderId = new Guid("1144423c-1a45-476d-8a9b-abdd3ae6c192"),
                    ProductVariantId = 5,
                    UserId = new Guid("b2d4c6e7-f89a-45bc-a123-4d56e78f9012"),
                    Comment = "Sản phẩm đẹp nhưng giá hơi cao.",
                    Rating = 3.5
                }
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
                    Name = "Khuyến mãi khai trương",    
                    Percent = 10,
                    Description = "Giảm 10% mừng khai trương",
                    IsActive = true,
                    DateStart = defaultDate,
                    DateEnd = defaultDate.AddMonths(1)
                },
                new Discount
                {
                    Id = 2,
                    Name = "Giảm giá mùa hè",
                    Percent = 15,
                    Description = "Ưu đãi mùa hè",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(5),
                    DateEnd = defaultDate.AddMonths(2)
                },
                new Discount
                {
                    Id = 3,
                    Name = "Giảm giá cuối tuần",
                    Percent = 20,
                    Description = "Giảm giá sốc cuối tuần",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(10),
                    DateEnd = defaultDate.AddMonths(1).AddDays(10)
                },
                new Discount
                {
                    Id = 4,
                    Name = "Khuyến mãi cho khách hàng thân thiết",
                    Percent = 5,
                    Description = "Giảm nhẹ cho khách quen",
                    IsActive = true,
                    DateStart = defaultDate.AddDays(3),
                    DateEnd = defaultDate.AddMonths(3)
                },
                new Discount
                {
                    Id = 5,
                    Name = "Giảm giá Black Friday",
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
