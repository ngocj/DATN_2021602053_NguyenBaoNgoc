using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_OrderDetail_OrderId_ProductVariantId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_OrderDetail_OrderId_ProductVariantId",
                table: "Feedback",
                columns: new[] { "OrderId", "ProductVariantId" },
                principalTable: "OrderDetail",
                principalColumns: new[] { "OrderId", "ProductVariantId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_OrderDetail_OrderId_ProductVariantId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_OrderDetail_OrderId_ProductVariantId",
                table: "Feedback",
                columns: new[] { "OrderId", "ProductVariantId" },
                principalTable: "OrderDetail",
                principalColumns: new[] { "OrderId", "ProductVariantId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
