using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentralizedSalesSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderItemId",
                table: "ServiceCharges",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DiscountId",
                table: "OrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCharges_OrderItemId",
                table: "ServiceCharges",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCharges_OrderItems_OrderItemId",
                table: "ServiceCharges",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCharges_OrderItems_OrderItemId",
                table: "ServiceCharges");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCharges_OrderItemId",
                table: "ServiceCharges");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ServiceCharges");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "OrderItems");
        }
    }
}
