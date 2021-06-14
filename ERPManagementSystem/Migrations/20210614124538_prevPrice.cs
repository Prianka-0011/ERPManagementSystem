using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPManagementSystem.Migrations
{
    public partial class prevPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousPrice",
                table: "PurchaseOrderLineItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousPrice",
                table: "PurchaseOrderLineItems");
        }
    }
}
