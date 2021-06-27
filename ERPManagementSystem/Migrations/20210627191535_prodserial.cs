using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class prodserial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OderNo",
                table: "SaleOrders");

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                table: "SaleOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductSerial",
                table: "SaleOrderItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductSerial",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "SaleOrders");

            migrationBuilder.DropColumn(
                name: "ProductSerial",
                table: "SaleOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductSerial",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "OderNo",
                table: "SaleOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
