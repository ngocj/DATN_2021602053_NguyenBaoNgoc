using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Discount",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Discount",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Category",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brand",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brand",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thương hiệu thể thao nổi tiếng" });

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thương hiệu đến từ Đức" });

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nổi bật với thiết kế năng động" });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "BrandName", "CreatedAt", "Description" },
                values: new object[,]
                {
                    { 4, "Under Armour", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyên đồ thể thao hiệu suất cao" },
                    { 5, "New Balance", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nổi bật với giày chạy bộ" }
                });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Các loại giày thể thao" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Áo thể thao nam nữ" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quần thể thao, quần short" });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "Description" },
                values: new object[,]
                {
                    { 4, "Phụ kiện", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Túi, nón, tất thể thao" },
                    { 5, "Đồ tập gym", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trang phục và dụng cụ tập gym" }
                });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart", "Description", "IsActive" },
                values: new object[] { new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giảm 10% mừng khai trương", true });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateEnd", "DateStart", "Description", "IsActive", "Percent" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ưu đãi mùa hè", true, 15 });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateEnd", "DateStart", "Description", "IsActive", "Percent" },
                values: new object[] { new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giảm giá sốc cuối tuần", true, 20 });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "Id", "DateEnd", "DateStart", "Description", "IsActive", "Percent" },
                values: new object[,]
                {
                    { 4, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giảm nhẹ cho khách quen", true, 5 },
                    { 5, new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday Sale", true, 25 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brand");

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Brand",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateEnd", "DateStart" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateEnd", "DateStart", "Percent" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20 });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateEnd", "DateStart", "Percent" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30 });
        }
    }
}
