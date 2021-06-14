using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class saleprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "PurchaseOrderLineItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "PurchaseOrderLineItems");
        }
    }
}
